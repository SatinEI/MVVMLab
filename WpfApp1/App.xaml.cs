using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Services;
using WpfApp1.ViewModel;
using WpfApp1.View;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            var connectionString = "Data Source=satinei;Initial Catalog=PhoneBookDB_SatinEI_2307b2;Integrated Security=true;TrustServerCertificate=true";
            services.AddDbContextFactory<PhoneBookDbSatinEi2307b2Context>(options =>
                options.UseSqlServer(connectionString));
            // Сервисы
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels
            services.AddSingleton<ContactsListViewModel>();
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