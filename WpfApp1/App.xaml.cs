using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.View;
using WpfApp1.ViewModel;

namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            var connectionString = "Data Source=dbsrv\\ov2025;Initial Catalog=PhoneBookDB_SatinEI_2307b2;Integrated Security=true;TrustServerCertificate=true";
            services.AddDbContext<PhoneBookDbSatinEi2307b2Context>(options =>
                options.UseSqlServer(connectionString));

            // Сервисы
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels
            services.AddSingleton<ContactListViewModel>();
            services.AddTransient<ContactEditViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddSingleton<MainWindowViewModel>();

            // Главное окно (Shell)
            services.AddSingleton<MVVMWindow>(provider =>
            {
                var window = new MVVMWindow();
                window.DataContext = provider.GetRequiredService<MainWindowViewModel>();
                return window;
            });

            var serviceProvider = services.BuildServiceProvider();

            var mainWindow = serviceProvider.GetRequiredService<MVVMWindow>();
            mainWindow.Show();
        }
    }
}