using GreenhouseApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- СЕКЦИЯ СЕРВИСОВ (DI Container) ---

// 1. Настройка подключения к PostgreSQL
builder.Services.AddDbContext<GreenhouseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Поддержка контроллеров (для нашего CRUD из ЛР1)
builder.Services.AddControllers();

// 3. Настройка Swagger (визуальный GUI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Регистрация Healthchecks (требование ЛР3)
builder.Services.AddHealthChecks();

var app = builder.Build();

// --- СЕКЦИЯ ЗАПУСКА И МИГРАЦИЙ ---

// Автоматическое создание таблиц с логикой ожидания базы (фикс для ЛР2 и ЛР4)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<GreenhouseDbContext>();
    
    int retries = 10; // Даем базе 10 попыток проснуться
    while (retries > 0)
    {
        try
        {
            Console.WriteLine("Попытка применить миграции к базе данных...");
            context.Database.Migrate();
            Console.WriteLine("Успех! База готова к работе.");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            if (retries == 0)
            {
                Console.WriteLine("Критическая ошибка: База данных не ответила после 10 попыток.");
                throw;
            }
            Console.WriteLine($"База еще не загружена. Ждем 3 секунды... (Осталось попыток: {retries})");
            Thread.Sleep(3000); 
        }
    }
}

// --- СЕКЦИЯ МАРШРУТОВ (Middleware) ---

// Включаем Swagger всегда (чтобы было удобно показывать преподавателю)
app.UseSwagger();
app.UseSwaggerUI();

// Основные маршруты контроллеров
app.MapControllers();

// Эндпоинт Healthcheck для робота на GitHub (ЛР3)
app.MapHealthChecks("/health");

// Эндпоинт для проверки балансировки (ЛР4)
// Показывает, какая именно копия приложения ответила на запрос
app.MapGet("/", () => 
{
    var nodeName = Environment.GetEnvironmentVariable("NODE_NAME") ?? "Локальная машина";
    return $"Привет! Это {nodeName}. Система работает исправно.";
});

app.Run();

// Эта строчка критически важна для работы автоматических тестов (ЛР3)
public partial class Program { }