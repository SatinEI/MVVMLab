// ViewModels/MainWindowViewModel.cs
using System.Windows.Input;
using WpfApp1.Services;

namespace WpfApp1.ViewModel
{
    public class MainWindowViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand ShowContactsCommand { get; }
        public ICommand ShowAboutCommand { get; }

        public INavigationService NavigationService => _navigationService;

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ShowContactsCommand = new RelayCommand(() => _navigationService.NavigateTo<ContactsListViewModel>());
            ShowAboutCommand = new RelayCommand(() => _navigationService.NavigateTo<AboutViewModel>());

            // При запуске сразу открыть список контактов
            _navigationService.NavigateTo<ContactsListViewModel>();
        }
    }
}