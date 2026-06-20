using MySqlConnector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Life4DunBackend.Infrastructure.Data;

/// <summary>
/// Controller để quản lý kết nối database
/// </summary>
public class DbConnectionController
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbConnectionController> _logger;

    public DbConnectionController(IConfiguration configuration, ILogger<DbConnectionController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Kiểm tra kết nối database
    /// </summary>
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("Connection string not found");
                return false;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                _logger.LogInformation("Database connection successful");
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Database connection failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Lấy connection string
    /// </summary>
    public string? GetConnectionString()
    {
        return _configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// Lấy thông tin database
    /// </summary>
    public async Task<DatabaseInfo?> GetDatabaseInfoAsync()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
                return null;

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                
                var builder = new MySqlConnectionStringBuilder(connectionString);
                return new DatabaseInfo
                {
                    Server = builder.Server,
                    Database = builder.Database,
                    IsConnected = true,
                    ServerVersion = connection.ServerVersion
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get database info: {ex.Message}");
            return null;
        }
    }
}

/// <summary>
/// Model thông tin database
/// </summary>
public class DatabaseInfo
{
    public string? Server { get; set; }
    public string? Database { get; set; }
    public bool IsConnected { get; set; }
    public string? ServerVersion { get; set; }
}
