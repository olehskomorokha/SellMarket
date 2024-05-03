using Microsoft.EntityFrameworkCore;
using SellMarket.Model.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var optionsBuilder = builder.Services.AddDbContext<StoreDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DeafaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
