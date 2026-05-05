using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using WpfApp1.ViewModel; // для ObservableObject

namespace WpfApp1.Services
{
    public class NavigationService : ObservableObject, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private object? _currentViewModel;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object? CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class
        {
            var vm = _serviceProvider.GetRequiredService<TViewModel>();

            if (vm is INavigationAware navigationAware)
                navigationAware.OnNavigatedTo(parameter);

            CurrentViewModel = vm;
        }
    }
}