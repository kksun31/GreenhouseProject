using GreenhouseApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Настройка подключения к БД
builder.Services.AddDbContext<GreenhouseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Добавляем поддержку контроллеров
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

// 3. Настраиваем Swagger (тот самый минимальный GUI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 1. Автоматически создаем таблицы в новой БД внутри Docker
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GreenhouseDbContext>();
    db.Database.Migrate();
}

// 2. Включаем Swagger для удобства
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// 4. Включаем Swagger, чтобы он работал в браузере
app.UseSwagger();
app.UseSwaggerUI();

// 5. Включаем маршрутизацию на наши контроллеры (без этого шага ничего не заработает)
app.MapControllers();

app.MapHealthChecks("/health");
app.Run();

public partial class Program { }