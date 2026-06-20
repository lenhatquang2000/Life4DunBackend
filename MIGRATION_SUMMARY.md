# Migration Summary: Users & Players Table

## ✅ Task Completed Successfully

Migration **`CreateUsersAndPlayersTable`** has been successfully created and applied to the MySQL database.

---

## 📋 What Was Done

### 1. **Created User Entity** (`Core/Entities/User.cs`)
   - Represents a system user account
   - Fields: `Id`, `Email`, `PasswordHash`, `FullName`, `CreatedAt`, `LastLoginAt`, `IsActive`
   - One User can have multiple Players (One-to-Many relationship)

### 2. **Updated Player Entity** (`Core/Entities/Player.cs`)
   - Now represents a character/player belonging to a User
   - Removed: `Email`, `PasswordHash` (moved to User)
   - Added: `UserId` (Foreign Key to User)
   - Fields: `Id`, `UserId`, `Username`, `Level`, `Experience`, `Gold`, `Gems`, `Health`, `MaxHealth`, `CreatedAt`, `LastLoginAt`, `IsActive`, `AvatarUrl`, `Attack`, `Defense`, `Speed`

### 3. **Updated GameDbContext** (`Infrastructure/Data/GameDbContext.cs`)
   - Added `DbSet<User>` 
   - Configured One-to-Many relationship: User → Players (Cascade delete)
   - Removed default value SQL for datetime fields (application will handle it)

### 4. **Updated DTOs** (`Core/DTOs/PlayerDto.cs`)
   - **PlayerDto**: Updated to remove Email, added UserId
   - **CreatePlayerDto**: Now only requires Username (User authentication is separate)
   - **RegisterDto**: New DTO for user registration (Email, Password, FullName)
   - **LoginDto**: Updated to use Email instead of Username
   - **AuthResponseDto**: Updated to return User info instead of Player info
   - **UserDto**: New DTO for User information with Players list

### 5. **Updated PlayersController** (`Api/Controllers/PlayersController.cs`)
   - Updated CreatePlayer to accept `userId` parameter
   - Removed Email and Password handling from Player creation
   - Updated all DTOs mapping

### 6. **Updated DatabaseSeeder** (`Infrastructure/Data/Seeders/DatabaseSeeder.cs`)
   - Seeds sample Users with multiple Players per User
   - Example: Admin user has 1 player, Player1 has 2 players, Player2 has 1 player

### 7. **Database Structure**

#### **Users Table**
```sql
CREATE TABLE `Users` (
    `Id` char(36) NOT NULL PRIMARY KEY,
    `Email` varchar(100) NOT NULL UNIQUE,
    `PasswordHash` longtext NOT NULL,
    `FullName` varchar(100) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `LastLoginAt` datetime(6) NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT TRUE
) CHARACTER SET utf8mb4;
```

#### **Players Table**
```sql
CREATE TABLE `Players` (
    `Id` char(36) NOT NULL PRIMARY KEY,
    `UserId` char(36) NOT NULL,
    `Username` varchar(50) NOT NULL UNIQUE,
    `Level` int NOT NULL DEFAULT 1,
    `Experience` bigint NOT NULL DEFAULT 0,
    `Gold` bigint NOT NULL DEFAULT 100,
    `Gems` bigint NOT NULL DEFAULT 0,
    `Health` int NOT NULL DEFAULT 100,
    `MaxHealth` int NOT NULL DEFAULT 100,
    `CreatedAt` datetime(6) NOT NULL,
    `LastLoginAt` datetime(6) NOT NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT TRUE,
    `AvatarUrl` longtext NULL,
    `Attack` int NOT NULL DEFAULT 10,
    `Defense` int NOT NULL DEFAULT 5,
    `Speed` int NOT NULL DEFAULT 10,
    FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;
```

#### **GameSessions Table**
```sql
CREATE TABLE `GameSessions` (
    `Id` char(36) NOT NULL PRIMARY KEY,
    `PlayerId` char(36) NOT NULL,
    `StartedAt` datetime(6) NOT NULL,
    `EndedAt` datetime(6) NULL,
    `Status` varchar(20) NOT NULL DEFAULT 'InProgress',
    `Score` int NOT NULL DEFAULT 0,
    `WavesCompleted` int NOT NULL DEFAULT 0,
    `RewardGold` bigint NOT NULL DEFAULT 0,
    `RewardExperience` bigint NOT NULL DEFAULT 0,
    `DifficultyLevel` longtext NULL,
    `EnemiesDefeated` int NOT NULL DEFAULT 0,
    `DeviceInfo` longtext NULL
) CHARACTER SET utf8mb4;
```

---

## 🔑 Key Features

✅ **One User → Many Players**: Một user có thể có nhiều nhân vật (players)
✅ **Separate Authentication**: Users quản lý authentication, Players quản lý game data
✅ **Cascade Delete**: Khi xóa User, tất cả Players của User đó sẽ bị xóa
✅ **Unique Constraints**: Email (Users) và Username (Players) đều là unique
✅ **Indexes**: Tối ưu hóa queries với indexes trên UserId, Username, Email, PlayerId, StartedAt
✅ **UTF8MB4**: Hỗ trợ đầy đủ Unicode characters

---

## 📊 Sample Data Structure

```
User (admin@life4dun.com)
├── Player "Admin" (Level 50, 1M EXP)
│   └── GameSessions: 5 sessions
│
User (player1@life4dun.com)
├── Player "Player1" (Level 25, 150K EXP)
│   └── GameSessions: 5 sessions
├── Player "Player1_Archer" (Level 15, 50K EXP)
│   └── GameSessions: (auto-generated)
│
User (player2@life4dun.com)
└── Player "Player2" (Level 18, 80K EXP)
    └── GameSessions: 5 sessions
```

---

## 🔄 Next Steps

1. **Create Auth Controller** - Implement user registration and login endpoints
2. **Update API Routes** - Adjust routes to use User ID instead of Player ID for authentication
3. **Implement JWT** - Use JWT tokens for API authentication
4. **Update Play API** - Modify player endpoints to require User authentication first
5. **Add User DTOs Controller** - Create endpoints for managing users and their players

---

## 📝 Migration Details

- **Migration ID**: `20260620013728_CreateUsersAndPlayersTable`
- **Status**: ✅ Applied
- **Database**: `Life4DunDb`
- **Tables Created**: Users, Players, GameSessions
- **Indexes Created**: 6 indexes (Email, Username, UserId, PlayerId, StartedAt)

