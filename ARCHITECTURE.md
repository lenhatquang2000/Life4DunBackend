# 🏗️ Life4DunBackend - Architecture Overview

## Project Structure (Clean Architecture)

```
Life4DunBackend/
├── src/
│   ├── Life4DunBackend.Api/                 # Presentation Layer (Web API)
│   │   ├── Controllers/
│   │   │   ├── PlayersController.cs         # Player management endpoints
│   │   │   └── DatabaseController.cs        # Database management (seed, reset)
│   │   ├── Properties/
│   │   │   └── launchSettings.json
│   │   ├── appsettings.json                 # Configuration
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.Production.json
│   │   ├── Program.cs                       # App startup & DI configuration
│   │   └── Life4DunBackend.Api.csproj
│   │
│   ├── Life4DunBackend.Core/                # Business Logic Layer (Domain)
│   │   ├── Entities/                        # Domain models
│   │   │   ├── User.cs                      # User account (authentication)
│   │   │   ├── Player.cs                    # Player/character (game data)
│   │   │   └── GameSession.cs               # Game session data
│   │   ├── DTOs/                            # Data Transfer Objects
│   │   │   └── PlayerDto.cs                 # Includes: PlayerDto, UserDto, CreatePlayerDto, RegisterDto, LoginDto, AuthResponseDto
│   │   └── Life4DunBackend.Core.csproj
│   │
│   └── Life4DunBackend.Infrastructure/      # Data Access Layer (Persistence)
│       ├── Data/
│       │   ├── GameDbContext.cs             # EF Core DbContext (entity configurations)
│       │   ├── Migrations/                  # Database migrations
│       │   │   ├── 20260620013728_CreateUsersAndPlayersTable.cs
│       │   │   ├── 20260620013728_CreateUsersAndPlayersTable.Designer.cs
│       │   │   └── GameDbContextModelSnapshot.cs
│       │   └── Seeders/                     # Data seeding
│       │       ├── DatabaseSeeder.cs        # Seeding logic
│       │       └── SeederExtensions.cs      # DI extension methods
│       └── Life4DunBackend.Infrastructure.csproj
│
├── tests/
│   └── Life4DunBackend.Tests/               # Unit tests
│
├── .env                                     # Environment variables
├── .env.example                             # Environment template
├── appsettings.json                         # Global settings
├── Life4DunBackend.sln                      # Solution file
├── MYSQL_SETUP.md                           # MySQL setup guide
├── PROJECT_STRUCTURE.md                     # Project structure documentation
└── README.md                                # Project documentation
```

---

## 📊 Database Schema

### **Users Table** (Authentication & User Management)
```sql
┌─────────────┐
│    Users    │
├─────────────┤
│ Id (PK)     │
│ Email       │ UNIQUE
│ PasswordHash│
│ FullName    │
│ CreatedAt   │
│ LastLoginAt │
│ IsActive    │
└─────────────┘
```

### **Players Table** (Game Characters)
```sql
┌──────────────────┐
│    Players       │
├──────────────────┤
│ Id (PK)          │
│ UserId (FK)      │─────┐ CASCADE DELETE
│ Username         │ UNIQUE
│ Level            │
│ Experience       │
│ Gold             │
│ Gems             │
│ Health/MaxHealth │
│ Attack/Defense   │
│ Speed            │
│ AvatarUrl        │
│ CreatedAt        │
│ LastLoginAt      │
│ IsActive         │
└──────────────────┘
       │
       └─────────────────┐
                         │
              ┌──────────────────┐
              │  GameSessions    │
              ├──────────────────┤
              │ Id (PK)          │
              │ PlayerId (FK)    │
              │ StartedAt        │
              │ EndedAt          │
              │ Status           │
              │ Score            │
              │ WavesCompleted   │
              │ RewardGold       │
              │ RewardExperience │
              │ EnemiesDefeated  │
              │ DifficultyLevel  │
              │ DeviceInfo       │
              └──────────────────┘
```

### **Relationships**
```
1 User ──────── 1..* Players  (One-to-Many)
  │                    │
  │                    └────── 1..* GameSessions (One-to-Many)
  │
  └─ ON DELETE CASCADE
     (Xóa User → Xóa Players → Xóa GameSessions)
```

---

## 🔧 Technology Stack

| Layer | Technology | Purpose |
|-------|-----------|---------|
| **API** | ASP.NET Core 8.0 | REST API framework |
| **Database** | MySQL 8.0 | Relational database |
| **ORM** | Entity Framework Core 8.0.0 | Data access & migrations |
| **MySQL Provider** | Pomelo.EntityFrameworkCore.MySql 8.0.0 | MySQL connectivity |
| **Authentication** | BCrypt.Net-Next 4.0.3 | Password hashing |
| **Logging** | Microsoft.Extensions.Logging | Application logging |
| **DI Container** | Microsoft.Extensions.DependencyInjection | Dependency injection |

---

## 🎯 Key Architecture Decisions

### 1. **Clean Architecture (Layered)**
- **Presentation Layer** (Api): Controllers, endpoints
- **Business Logic Layer** (Core): Entities, DTOs, business rules
- **Data Access Layer** (Infrastructure): DbContext, migrations, seeding

### 2. **One User → Many Players**
- Supports multiple game characters per user
- User manages authentication
- Players manage game data
- Cascade delete ensures data consistency

### 3. **Repository Pattern** (Preparation)
- Current: Direct DbContext in controllers (simple projects)
- Future: Abstract with IRepository<T> for testability

### 4. **Database Migrations**
- Version controlled in `Infrastructure/Data/Migrations/`
- Single migration: `CreateUsersAndPlayersTable`
- Tracked in `__EFMigrationsHistory` table

### 5. **Data Seeding**
- Separated into `DatabaseSeeder.cs`
- Extension method for clean DI setup
- Runs on application startup (in Program.cs)

---

## 📝 Configuration Management

### **appsettings.json** (Shared)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=Life4DunDb;User=root;Password=;"
  },
  "Jwt": { ... },
  "GameSettings": { ... }
}
```

### **.env** (Local secrets)
```env
DB_SERVER=localhost
DB_PORT=3306
DB_NAME=Life4DunDb
DB_USER=root
DB_PASSWORD=

API_PORT=5000
JWT_SECRET_KEY=...
```

---

## 🚀 Current Implementation Status

### ✅ Completed
- [x] Project structure (Clean Architecture)
- [x] Database design (Users, Players, GameSessions)
- [x] EF Core migrations
- [x] Entity configurations
- [x] Data seeding
- [x] DTOs layer
- [x] PlayersController (basic CRUD)
- [x] MySQL integration

### ⏳ Pending (Next Phase)
- [ ] AuthController (Login, Register)
- [ ] Authentication middleware (JWT)
- [ ] Authorization filters
- [ ] GameSessionController
- [ ] Leaderboard endpoints
- [ ] Error handling middleware
- [ ] Unit tests
- [ ] API documentation (Swagger)

---

## 📚 Key Files

| File | Purpose |
|------|---------|
| `Program.cs` | Application startup & DI registration |
| `GameDbContext.cs` | EF Core configurations & entity mappings |
| `User.cs` | User entity (Domain model) |
| `Player.cs` | Player entity (Domain model) |
| `GameSession.cs` | GameSession entity (Domain model) |
| `PlayerDto.cs` | DTOs for API serialization |
| `PlayersController.cs` | Player management endpoints |
| `DatabaseSeeder.cs` | Sample data initialization |
| `CreateUsersAndPlayersTable.cs` | Initial database schema |

---

## 🔐 Security Considerations

- ✅ Passwords hashed with BCrypt
- ✅ Email uniqueness enforced
- ✅ Database cascade rules in place
- ⏳ JWT authentication (pending)
- ⏳ Role-based authorization (pending)
- ⏳ Input validation (pending)

---

## 📈 Scalability Notes

**Current Design Supports:**
- Multiple players per user
- Game session history tracking
- User activity logging (LastLoginAt)
- Player statistics (Level, EXP, Gold, Gems)
- Character customization (AvatarUrl)

**Future Improvements:**
- Repository pattern abstraction
- Caching layer (Redis)
- Async data access
- Background jobs (Hangfire)
- Event sourcing
- CQRS pattern

---

## ✨ Best Practices Followed

1. **Separation of Concerns**: Clear layering
2. **DRY Principle**: DTOs for serialization, entities for storage
3. **SOLID Principles**: Single responsibility, dependency injection
4. **Database Migrations**: Version controlled schema changes
5. **Naming Conventions**: Clear, descriptive names
6. **Code Organization**: Logical folder structure
7. **Configuration Management**: Environment-specific settings

