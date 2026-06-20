# Hướng dẫn cài đặt MySQL cho Life4DunBackend

## Yêu cầu

- MySQL Server 8.0 trở lên
- MySQL Workbench (optional, để quản lý GUI)
- .NET 8 SDK

---

## Bước 1: Cài đặt MySQL Server

### Windows:

1. Tải MySQL Community Server từ: https://dev.mysql.com/downloads/mysql/
2. Chọn phiên bản **MySQL 8.0** (LTS)
3. Download installer và chạy
4. Trong quá trình setup:
   - **Port**: 3306 (mặc định)
   - **Root Password**: Để trống hoặc nhập password
   - Chọn "Configure MySQL as a Windows Service"
5. Hoàn tất cài đặt

### macOS:
```bash
brew install mysql
mysql.server start
```

### Linux (Ubuntu/Debian):
```bash
sudo apt update
sudo apt install mysql-server
sudo mysql_secure_installation
```

---

## Bước 2: Tạo Database

### Cách 1: Dùng MySQL Command Line

```bash
mysql -u root
```

Sau đó chạy các lệnh SQL:

```sql
-- Tạo database
CREATE DATABASE Life4DunDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Kiểm tra database đã tạo
SHOW DATABASES;

-- Thoát
EXIT;
```

### Cách 2: Dùng MySQL Workbench

1. Mở MySQL Workbench
2. Kết nối với MySQL Server (localhost:3306, user: root)
3. Vào "File" → "New Query Tab"
4. Chạy SQL commands ở trên

---

## Bước 3: Cấu hình Connection String

### File: `.env`

```
DB_SERVER=localhost
DB_PORT=3306
DB_NAME=Life4DunDb
DB_USER=root
DB_PASSWORD=          # Để trống nếu root không có password
```

Nếu root có password:
```
DB_PASSWORD=your_password_here
```

### File: `appsettings.Development.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=Life4DunDb;User=root;Password=;"
}
```

Nếu có password:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=Life4DunDb;User=root;Password=your_password;"
}
```

---

## Bước 4: Restore Project Dependencies

```bash
cd Life4DunBackend
dotnet restore
```

---

## Bước 5: Chạy Database Migrations

```bash
dotnet ef database update --project src/Life4DunBackend.Infrastructure
```

Hoặc từ project API:
```bash
cd src/Life4DunBackend.Api
dotnet ef database update
```

Lệnh này sẽ:
- Tạo bảng Players
- Tạo bảng GameSessions
- Tạo các indexes

---

## Bước 6: Khởi động ứng dụng

```bash
dotnet run --project src/Life4DunBackend.Api
```

Lần đầu chạy:
- Ứng dụng sẽ chạy migrations
- Tự động seed dữ liệu mẫu (3 players + game sessions)

---

## Kiểm tra Database

### Dùng MySQL CLI:

```bash
mysql -u root
USE Life4DunDb;
SELECT * FROM Players;
SELECT * FROM GameSessions;
```

### Dùng MySQL Workbench:

1. Mở kết nối (localhost:3306, root)
2. Vào Schemas → Life4DunDb
3. Xem tables trong lệnh `Tables` folder

---

## Troubleshooting

### Lỗi: "Can't connect to MySQL server"

**Nguyên nhân:**
- MySQL service chưa khởi động
- Port 3306 đã bị chiếm

**Cách khắc phục:**

Windows:
```bash
# Kiểm tra MySQL service
net start MySQL80

# Nếu lỗi, khởi động lại
net stop MySQL80
net start MySQL80
```

macOS:
```bash
mysql.server start
```

Linux:
```bash
sudo systemctl start mysql
sudo systemctl status mysql
```

---

### Lỗi: "Access denied for user 'root'@'localhost'"

**Nguyên nhân:** Password sai hoặc chưa tạo database

**Cách khắc phục:**
1. Kiểm tra `.env` và `appsettings.json`
2. Đảm bảo database `Life4DunDb` đã được tạo
3. Reset password root:

```bash
mysql -u root -p
ALTER USER 'root'@'localhost' IDENTIFIED BY '';
FLUSH PRIVILEGES;
```

---

### Lỗi: "Duplicate entry for key 'Username'"

**Nguyên nhân:** Dữ liệu seeding bị trùng lặp

**Cách khắc phục:**
```bash
# Xóa database
dotnet ef database drop --force --project src/Life4DunBackend.Infrastructure

# Tạo lại
dotnet ef database update --project src/Life4DunBackend.Infrastructure
```

---

### Lỗi: "Column count doesn't match"

**Nguyên nhân:** Migration bị lỗi

**Cách khắc phục:**
```bash
# Xóa database
mysql -u root -e "DROP DATABASE Life4DunDb;"
mysql -u root -e "CREATE DATABASE Life4DunDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;"

# Chạy migration lại
dotnet ef database update --project src/Life4DunBackend.Infrastructure
```

---

## Database Schema

### Players Table

| Column | Type | Default | Constraint |
|--------|------|---------|-----------|
| Id | CHAR(36) UUID | - | PK |
| Username | VARCHAR(50) | - | UNIQUE, NOT NULL |
| Email | VARCHAR(100) | - | UNIQUE, NOT NULL |
| PasswordHash | LONGTEXT | - | NOT NULL |
| Level | INT | 1 | - |
| Experience | BIGINT | 0 | - |
| Gold | BIGINT | 100 | - |
| Gems | BIGINT | 0 | - |
| Health | INT | 100 | - |
| MaxHealth | INT | 100 | - |
| CreatedAt | DATETIME(6) | - | - |
| LastLoginAt | DATETIME(6) | - | - |
| IsActive | BOOLEAN | true | - |
| AvatarUrl | LONGTEXT | - | NULL |
| Attack | INT | 10 | - |
| Defense | INT | 5 | - |
| Speed | INT | 10 | - |

### GameSessions Table

| Column | Type | Default | Constraint |
|--------|------|---------|-----------|
| Id | CHAR(36) UUID | - | PK |
| PlayerId | CHAR(36) UUID | - | FK(Players.Id) |
| StartedAt | DATETIME(6) | - | - |
| EndedAt | DATETIME(6) | - | NULL |
| Status | VARCHAR(20) | 'InProgress' | - |
| Score | INT | 0 | - |
| WavesCompleted | INT | 0 | - |
| RewardGold | BIGINT | 0 | - |
| RewardExperience | BIGINT | 0 | - |
| DifficultyLevel | LONGTEXT | - | NULL |
| EnemiesDefeated | INT | 0 | - |
| DeviceInfo | LONGTEXT | - | NULL |

---

## Backup & Restore Database

### Backup:
```bash
mysqldump -u root -p Life4DunDb > backup.sql
```

### Restore:
```bash
mysql -u root -p Life4DunDb < backup.sql
```

---

## API Endpoints

### Database Health Check
```
GET /api/database/health
```

Response:
```json
{
  "status": "Connected",
  "message": "Database connection is healthy"
}
```

### Get Database Info
```
GET /api/database/info
```

Response:
```json
{
  "server": "localhost",
  "database": "Life4DunDb",
  "isConnected": true,
  "serverVersion": "8.0.x"
}
```

---

## Liên hệ & Support

Nếu có vấn đề, kiểm tra:
- MySQL Server đang chạy
- Connection string đúng
- Database `Life4DunDb` đã được tạo
- User `root` có quyền truy cập

