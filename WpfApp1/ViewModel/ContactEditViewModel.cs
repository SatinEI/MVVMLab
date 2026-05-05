// ViewModels/ContactEditViewModel.cs
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.Services;

namespace WpfApp1.ViewModel
{
    public class ContactEditViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private Contact? _contact;

        public string EditName
        {
            get => _contact?.Name ?? string.Empty;
            set { if (_contact != null) { _contact.Name = value; OnPropertyChanged(); } }
        }

        public string EditPhone
        {
            get => _contact?.Phone ?? string.Empty;
            set { if (_contact != null) { _contact.Phone = value; OnPropertyChanged(); } }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ContactEditViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void OnNavigatedTo(object? parameter)
        {
            if (parameter is Contact contact)
                _contact = contact;
            else
                _contact = null; // или создать новый

            // Обновить свойства для UI
            OnPropertyChanged(nameof(EditName));
            OnPropertyChanged(nameof(EditPhone));
        }

        private void Save()
        {
            // Здесь можно добавить валидацию или сохранение в репозиторий
            _navigationService.NavigateTo<ContactListViewModel>();
        }

        private void Cancel()
        {
            _navigationService.NavigateTo<ContactListViewModel>();
        }
    }
}