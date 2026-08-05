# SellMarket

A RESTful backend API for an online marketplace where users can register, authenticate, and publish product listings with images. Built with **ASP.NET Core 8**, the project follows a layered architecture with clear separation between controllers, services, and data access.

---

## Overview

**SellMarket** is a marketplace API that powers user accounts, product catalog browsing, and seller workflows. Users can sign up, log in, manage their profile, and post items for sale. Buyers can browse categories, search and filter listings, and view product details.

The API is designed to be consumed by a separate frontend (CORS is configured for local React and static dev servers).

---

## Features

### Authentication & Users
- User registration with validated input
- Login with **JWT** token issuance (60-day expiry)
- Password hashing via **BCrypt** (salt rounds: 12)
- Protected endpoints using `[Authorize]` and claim-based identity (`ClaimTypes.Email`)
- Profile management: update name, email, phone, nickname, and address

### Products & Catalog
- Full product CRUD (create, read, update, delete)
- Hierarchical **categories and subcategories** (self-referencing `ProductCategory`)
- Browse all products, filter by subcategory, search by title
- Sort and filter by price range (`minPrice`, `maxPrice`, `sortType`)
- View latest listings and authenticated user's own posts
- Multi-image upload on product creation (stored as comma-separated URLs)

### Image Storage
- Product images uploaded to **Google Drive** via the Drive API v3
- Files are made publicly readable and served as thumbnail URLs
- Images are deleted from Drive when a product is removed

### Developer Experience
- **Swagger UI** with JWT Bearer security scheme for interactive API testing
- **EF Core migrations** for schema versioning
- Custom domain exceptions (`UserNotFoundException`, `ProductNotFoundException`, etc.)
- DTO mappers (`ProductMapper`, `UserMapper`) to decouple entities from API responses

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Runtime | .NET 8 |
| Web framework | ASP.NET Core Web API |
| ORM | Entity Framework Core 8 |
| Database | Microsoft SQL Server |
| Authentication | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Password hashing | BCrypt.Net-Next |
| File storage | Google Drive API v3 (`Google.Apis.Drive.v3`) |
| API docs | Swashbuckle (Swagger/OpenAPI) |
| IDE | JetBrains Rider / Visual Studio 2022 |

---

## Architecture

```
SellMarket/
├── Controllers/          # HTTP endpoints (UserController, ProductController)
├── Services/             # Business logic (UserService, ProductService, ImageService)
├── Interfaces/           # Service contracts (ICrud<T>, IUserService, IProductService)
├── Model/
│   ├── Entities/         # EF Core domain models (User, Product, ProductCategory, Order)
│   ├── Models/           # Request/response DTOs
│   ├── Mappers/          # Entity ↔ DTO mapping
│   └── Data/             # StoreDbContext
├── Exceptions/           # Custom exception types
└── Migrations/           # EF Core database migrations
```

**How it works:**

1. **Controllers** receive HTTP requests and delegate to injected services.
2. **Services** implement business rules, call `StoreDbContext` for persistence, and use mappers to shape responses.
3. **`ICrud<T>`** provides a reusable CRUD contract shared by `UserService` and `ProductService`.
4. **`ImageService`** handles OAuth2 refresh-token flow with Google Drive, uploads files, sets public permissions, and returns thumbnail URLs.
5. **`UserService`** reads the authenticated user's email from `IHttpContextAccessor` JWT claims to scope operations (e.g., "my posts", profile updates).

---

## Data Model

| Entity | Description |
|--------|-------------|
| `User` | Seller/buyer account (name, email, hashed password, contact info) |
| `Product` | Listing (title, description, price, images, category, seller) |
| `ProductCategory` | Category tree with optional `ParentCategoryId` for subcategories |
| `ProductDetails` | Extended product metadata |
| `Order` | Order entity with state enum (Open, Confirmed, Delivered, Closed) |

Schema evolved through migrations — for example, the separate `UserAdresses` table was refactored into an inline `Address` field on `User`.

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB, Express, or full instance)
- Google Cloud project with Drive API enabled and OAuth credentials
- JetBrains Rider, Visual Studio, or VS Code

### Configuration

Create `SellMarket/appsettings.json` (this file is gitignored for security):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SellMarketDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-at-least-32-characters-long",
    "Issuer": "SellMarket",
    "Audience": "SellMarketClients"
  },
  "GoogleDriveApi": {
    "ClientId": "your-google-client-id",
    "ClientSecret": "your-google-client-secret",
    "refresh_token": "your-oauth-refresh-token",
    "folderId": "your-drive-folder-id"
  },
  "GoogleCloud": {
    "BucketName": "your-bucket-name",
    "ServiceAccountKeyPath": "path/to/service-account-key.json"
  }
}
```

Alternatively, use [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) (already configured in the project):

```bash
cd SellMarket
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=..."
dotnet user-secrets set "Jwt:Key" "your-secret-key"
```

### Database Setup

```bash
cd SellMarket
dotnet ef database update
```

### Run the API

```bash
dotnet run --project SellMarket
```

The API starts at:
- HTTP: `http://localhost:5220`
- HTTPS: `https://localhost:7118`
- Swagger UI: `http://localhost:5220/swagger` (Development only)

---

## API Endpoints

### Users — `/api/User`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `/api/User` | — | List all users |
| `GET` | `/api/User/{id}` | — | Get user by ID |
| `POST` | `/api/User/Register` | — | Register a new account |
| `POST` | `/api/User/login` | — | Login and receive JWT |
| `GET` | `/api/User/getUserInfo` | JWT | Get current user profile |
| `GET` | `/api/User/getMyEmail` | JWT | Get authenticated user's email |
| `PUT` | `/api/User/addUserAddress` | JWT | Update contact/address info |
| `PUT` | `/api/User/UpdateUserSettings` | JWT | Update profile settings |

### Products — `/api/Product`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `/api/Product/GetAllProduct` | — | List all products |
| `GET` | `/api/Product/GetProductsBySubcategoryId?id=` | — | Products in a subcategory |
| `GET` | `/api/Product/GetProductCategory` | — | Top-level categories |
| `GET` | `/api/Product/GetAllSubcategory` | — | All subcategories |
| `GET` | `/api/Product/GetSubcategoriesByCategoryId?id=` | — | Subcategories by parent |
| `GET` | `/api/Product/GetProductByTitle?keyWord=` | — | Search by title |
| `GET` | `/api/Product/GetProductsById?id=` | — | Product detail view |
| `GET` | `/api/Product/GetProductsBySubcategoryWithFilterId` | — | Filter/sort by price |
| `GET` | `/api/Product/GetProductImg?productId=` | — | Product image URLs |
| `GET` | `/api/Product/GetNewProduct` | — | Recently published items |
| `GET` | `/api/Product/GetUserPosts` | JWT | Current user's listings |
| `POST` | `/api/Product/addProduct` | JWT | Create listing (multipart/form-data) |
| `PUT` | `/api/Product/{id}` | JWT | Update a product |
| `DELETE` | `/api/Product/DeleteProduct/{id}` | JWT | Delete a product and its images |

### Authentication Header

Protected endpoints require:

```
Authorization: Bearer <your-jwt-token>
```

Use the **Authorize** button in Swagger UI to paste your token after logging in.

---

## CORS

The API allows requests from these origins (configured in `Program.cs`):

- `http://localhost:3000` — React dev server
- `http://192.168.56.1:3000` — LAN frontend
- `http://127.0.0.1:5500` — Live Server / static frontend

---

## What I Built

This project demonstrates:

- **REST API design** with resource-oriented controllers and consistent routing
- **JWT-based stateless authentication** with claim extraction from the HTTP context
- **Secure password storage** using BCrypt hashing instead of plain text
- **Entity Framework Core** with Code First migrations and relational modeling (foreign keys, self-referencing categories, includes/joins)
- **Third-party integration** — Google Drive OAuth2 flow for cloud file upload and cleanup
- **Dependency injection** — scoped services registered in `Program.cs`, interface-based abstractions
- **DTO pattern** — separate request/response models and static mappers to keep entities internal
- **Swagger/OpenAPI** documentation with Bearer token support for easy testing
- **Custom exception handling** for domain-specific error cases

---

## License

This project is for educational and portfolio purposes.
