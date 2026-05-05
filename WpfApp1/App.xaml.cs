using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Services;
using WpfApp1.ViewModel;
using WpfApp1.View;

namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Сервисы
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels
            services.AddTransient<ContactListViewModel>();
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