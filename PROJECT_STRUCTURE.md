# Cấu trúc Project Life4DunBackend

## Tổng quan

```
Life4DunBackend/
│
├── src/
│   ├── Life4DunBackend.Api/              # API Layer - Controllers & Endpoints
│   ├── Life4DunBackend.Core/             # Core Layer - Business Logic & Entities
│   └── Life4DunBackend.Infrastructure/   # Infrastructure Layer - Database & Services
│
├── tests/
│   └── Life4DunBackend.Tests/            # Unit & Integration Tests
│
├── .env                                  # Environment Variables (Development)
├── .env.example                          # Template for .env
├── Life4DunBackend.sln                   # Visual Studio Solution File
└── README.md                             # Project Documentation
```

---

## Chi tiết từng thư mục

### 1. **src/Life4DunBackend.Api/**
API Layer - Nơi định nghĩa các HTTP endpoints

```
Life4DunBackend.Api/
├── Controllers/              # Các API controllers
│   ├── PlayersController.cs  # API endpoints cho Players (CRUD, Leaderboard)
│   └── DatabaseController.cs # API endpoints để kiểm tra DB health
├── Middleware/               # Custom middleware (authentication, logging, etc.)
├── Services/                 # Business service implementations
├── Program.cs               # Application entry point
├── appsettings.json         # Default configuration
├── appsettings.Development.json    # Development environment config
├── appsettings.Production.json     # Production environment config
└── Life4DunBackend.Api.csproj      # Project file
```

**Trách nhiệm:**
- Định nghĩa API endpoints
- Xử lý HTTP requests/responses
- Validation input từ client
- Gọi services để thực hiện business logic

---

### 2. **src/Life4DunBackend.Core/**
Core Layer - Chứa entities và DTOs

```
Life4DunBackend.Core/
├── Entities/                 # Domain models
│   ├── Player.cs            # Player entity
│   └── GameSession.cs       # GameSession entity
├── DTOs/                    # Data Transfer Objects
│   └── PlayerDto.cs         # Player DTOs (Create, Response, etc.)
├── Interfaces/              # Service contracts
│   └── IPlayerService.cs    # Player service interface (future)
├── Constants/               # Application constants
│   └── GameConstants.cs     # Game-related constants
└── Life4DunBackend.Core.csproj
```

**Trách nhiệm:**
- Định nghĩa domain models (Entities)
- Định nghĩa Data Transfer Objects (DTOs) cho API
- Định nghĩa contracts cho services

---

### 3. **src/Life4DunBackend.Infrastructure/**
Infrastructure Layer - Database & External Services

```
Life4DunBackend.Infrastructure/
├── Data/
│   ├── GameDbContext.cs              # EF Core DbContext
│   ├── DbConnectionController.cs     # Database connection management
│   ├── Migrations/                   # Database migrations
│   │   ├── InitialMigration.cs       # Initial schema creation
│   │   └── GameDbContextModelSnapshot.cs  # EF snapshot
│   └── Seeders/                      # Database seeding
│       ├── DatabaseSeeder.cs         # Main seeder class
│       ├── SeederExtensions.cs       # Extensions cho DI & middleware
│       └── [Seed data tại đây]       # Dữ liệu mẫu cho mỗi entity
├── Repositories/                     # Data access layer (future)
│   └── PlayerRepository.cs           # Player CRUD operations
├── Services/                         # External services (future)
│   ├── AuthService.cs               # Authentication/JWT
│   └── EmailService.cs              # Email notifications
└── Life4DunBackend.Infrastructure.csproj
```

**Trách nhiệm:**
- Database connection & configuration
- Entity Framework Core DbContext
- Database migrations
- Data seeding
- Repository pattern for data access
- External service integrations

---

## Quy trình dữ liệu (Data Flow)

### Khi nhận HTTP Request:

```
HTTP Request
    ↓
[Controllers] (Life4DunBackend.Api)
    ↓
[Services] (Business Logic)
    ↓
[Repositories] (Data Access) (Life4DunBackend.Infrastructure)
    ↓
[DbContext] (EF Core)
    ↓
[SQL Server Database]
```

### Ví dụ: GET /api/players/1

1. **PlayersController.GetPlayer(id)** nhận request
2. Gọi **GameDbContext.Players.FindAsync(id)**
3. Chuyển đổi Entity → DTO
4. Return **200 OK** với PlayerDto

---

## Database Schema

### Players Table
```sql
CREATE TABLE Players (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Level INT,
    Experience BIGINT,
    Gold BIGINT,
    Gems BIGINT,
    Health INT,
    MaxHealth INT,
    CreatedAt DATETIME2,
    LastLoginAt DATETIME2,
    IsActive BIT,
    AvatarUrl NVARCHAR(MAX),
    Attack INT,
    Defense INT,
    Speed INT
);
```

### GameSessions Table
```sql
CREATE TABLE GameSessions (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    PlayerId UNIQUEIDENTIFIER NOT NULL,
    StartedAt DATETIME2,
    EndedAt DATETIME2 NULL,
    Status NVARCHAR(20) NOT NULL,
    Score INT,
    WavesCompleted INT,
    RewardGold BIGINT,
    RewardExperience BIGINT,
    DifficultyLevel NVARCHAR(MAX),
    EnemiesDefeated INT,
    DeviceInfo NVARCHAR(MAX),
    FOREIGN KEY (PlayerId) REFERENCES Players(Id)
);
```

---

## API Endpoints

### Players API
- `GET /api/players` - Lấy danh sách người chơi
- `GET /api/players/{id}` - Lấy thông tin 1 người chơi
- `POST /api/players` - Tạo người chơi mới
- `PUT /api/players/{id}` - Cập nhật người chơi
- `DELETE /api/players/{id}` - Xóa người chơi
- `GET /api/players/leaderboard/top` - Lấy top 10 người chơi

### Database API
- `GET /api/database/health` - Kiểm tra kết nối DB
- `GET /api/database/info` - Lấy thông tin DB

---

## Cách sử dụng

### 1. Khôi phục database
```bash
dotnet ef database update --project src/Life4DunBackend.Infrastructure
```

### 2. Tạo migration mới
```bash
dotnet ef migrations add {MigrationName} --project src/Life4DunBackend.Infrastructure
```

### 3. Xóa dữ liệu và reset database
```bash
dotnet ef database drop --project src/Life4DunBackend.Infrastructure
dotnet ef database update --project src/Life4DunBackend.Infrastructure
```

### 4. Chạy ứng dụng
```bash
dotnet run --project src/Life4DunBackend.Api
```

---

## Dependencies

- **Microsoft.EntityFrameworkCore** - ORM for database
- **Microsoft.EntityFrameworkCore.SqlServer** - SQL Server provider
- **Swashbuckle.AspNetCore** - Swagger/OpenAPI documentation
- **BCrypt.Net-Next** - Password hashing

---

## Next Steps

- [ ] Implement authentication (JWT)
- [ ] Add GameSessions controller
- [ ] Create repositories layer
- [ ] Add business services
- [ ] Implement caching (Redis)
- [ ] Add logging
- [ ] Write unit tests
- [ ] API versioning
- [ ] Rate limiting
- [ ] Validation & error handling improvement

