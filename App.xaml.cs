using DrugStoreStatistics.Data;
using DrugStoreStatistics.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DrugStoreStatistics;

public partial class App : Application
{
    private IHost _host = null!;

    // Глобальний кеш зображень для всього додатку
    public static readonly Dictionary<string, BitmapImage> ImageCache = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration
                    .GetConnectionString("DefaultConnection");

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddScoped<MedicineService>();
                services.AddScoped<CategoryService>();

                services.AddTransient<MainWindow>();
            })
            .Build();

        _host.Start();

        // Автоматично застосовуємо міграції при запуску
        using var scope = _host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}