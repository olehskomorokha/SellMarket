using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SellMarket.Exceptions;
using SellMarket.Exeptions;
using SellMarket.Model.Data;
using SellMarket.Model.Entities;
using SellMarket.Model.Mappers;
using SellMarket.Model.Models;

namespace SellMarket.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly StoreDbContext _context;
        readonly IConfiguration _configuration;

        public UserService (IHttpContextAccessor httpContextAccessor,StoreDbContext context,IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context; 
            _httpContextAccessor = httpContextAccessor;
        }
        
        // Crud
        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public async Task<User> Create(User product)
        {
            var newUser = await _context.Users.AddAsync(product);
            await _context.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task Update(User user)
        {
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int modelId)
        {
            var user = await _context.Users.FindAsync(modelId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UserNotFoundException();
            }
        }
        
        // /Crud
        public async Task UpdateUserModel(UpdateUserSettingsModel userModel)
        {
            var userEmail = GetMyEmail();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserEmail == userEmail);
            if (user != null)
            {
                if(!userModel.FirstName.IsNullOrEmpty() && userModel.FirstName != null)
                    user.FirstName = userModel.FirstName;
                if(!userModel.LastName.IsNullOrEmpty() && userModel.LastName != null)
                    user.LastName= userModel.LastName;
                if(!userModel.UserEmail.IsNullOrEmpty() && userModel.UserEmail != null)
                    user.UserEmail = userModel.UserEmail;
                if(!userModel.PhoneNumber.IsNullOrEmpty())
                    user.PhoneNumber = userModel.PhoneNumber;
                _context.Update(user);
            }

            await _context.SaveChangesAsync();
        }
        public string GetMyEmail()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email);
                result = claim?.Value.Trim();
            }
            return result;
        }

        public async Task<UserRegister> Register(UserRegister user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) ||
                string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new Exception("User data is null or empty.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == user.Email);
            if (existingUser != null)
            {
                throw new UserAlreadyExistException();
            }

            var hashedPassword = HashPassword(user.Password);

            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfRegistration = DateTime.Now,
                UserEmail = user.Email,
                Password = hashedPassword
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return UserMapper.MapToUserRegister(newUser);
        }

        public async Task<string> Login(UserLogin user)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(x => x.UserEmail == user.Email);
            if (userFromDb == null)
            {
                throw new LoginException("invalid Email", "Invalid email format");
            }
            
            if (!BCrypt.Net.BCrypt.Verify(user.Password, userFromDb.Password))
            {
                throw new LoginException("invalid Password", "Password is incorrect");
            }
            
            var token = GetAccessToken(userFromDb.Id, userFromDb.UserEmail);
            return token;
        }
    
        public async Task<UserInfoModel> GetUserInfo()
        {
            var userEmail = GetMyEmail();
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new IsNullOrEmptyException("isNullOrEmpty", "Email is null or empty");
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserEmail == userEmail);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return UserMapper.MapToUserInfoModel(user);
        }

        public async Task<User> AddUserAddress(UserContactModel userContactModel)
        {
            var userEmail = GetMyEmail();
    
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserEmail == userEmail);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            user.NickName = userContactModel.NickName;
            user.Address = userContactModel.Adress;
            user.PhoneNumber = userContactModel.Phone;
    
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
    
            return user;
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        public string GetAccessToken(int userId, string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email), 
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var jwt = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(60)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    } 
}

