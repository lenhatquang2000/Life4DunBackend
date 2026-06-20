using Life4DunBackend.Infrastructure.Data;
using Life4DunBackend.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Life4Dun Game Backend API",
        Version = "v1",
        Description = "API endpoints dành cho Game Client gọi và Debug hệ thống."
    });

    // AI_NOTE: Bật tính năng đọc comment XML từ Code để hiển thị chú thích chi tiết lên Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add DbContext with MySQL
var serverVersion = new MySqlServerVersion(new Version(8, 0));
builder.Services.AddDbContext<GameDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, serverVersion, mysqlOptions =>
    {
        mysqlOptions.UseMicrosoftJson();
    });
});

// Add Database Seeder
builder.Services.AddDatabaseSeeder();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUnity", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("EnableSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Life4Dun API v1");
        c.RoutePrefix = "swagger"; // Đường dẫn truy cập sẽ là: http://localhost:port/swagger
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowUnity");
app.UseAuthorization();
app.MapControllers();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();
    dbContext.Database.Migrate();
}

// Seed database với dữ liệu mẫu
await app.SeedDatabaseAsync();

app.Run();
