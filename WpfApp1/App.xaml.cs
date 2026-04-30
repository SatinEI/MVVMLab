using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using WpfApp1.Model;
using WpfApp1.View;
using WpfApp1.ViewModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Создаём коллекцию сервисов
            var services = new ServiceCollection();

            // 2. Регистрируем сервисы
            // DialogService — Singleton, так как он не хранит
            // состояние пользователя.
            services.AddSingleton<IDialogService, DialogService>();

            // 3. ViewModel — Transient (при навигации нам будут
            // нужны новые экземпляры)
            services.AddTransient<ViewModels>();

            // 4. Главное окно — Singleton с явной передачей
            // DataContext через лямбда-выражение
            services.AddSingleton<MVVMWindow>(provider =>
            {
                var window = new MVVMWindow();
                window.DataContext = provider.GetRequiredService<ViewModels>();
                return window;
            });

            // 5. Создаём контейнер (ServiceProvider)
            var serviceProvider = services.BuildServiceProvider();

            // 6. Получаем главное окно и запускаем его
            var mainWindow = serviceProvider.GetRequiredService<MVVMWindow>();
            mainWindow.Show();
        }
    }

}
