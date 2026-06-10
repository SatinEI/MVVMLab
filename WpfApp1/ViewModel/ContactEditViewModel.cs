using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModel
{
    public class ContactEditViewModel : ObservableObject, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly PhoneBookDbSatinEi2307b2Context _context;

        private Contact? _contact;
        private bool _isNewContact;

        public string EditName
        {
            get => _contact?.Name ?? string.Empty;
            set
            {
                if (_contact != null)
                {
                    _contact.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EditPhone
        {
            get => _contact?.Phone ?? string.Empty;
            set
            {
                if (_contact != null)
                {
                    _contact.Phone = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ContactEditViewModel(
            PhoneBookDbSatinEi2307b2Context context,
            INavigationService navigationService)
        {
            _context = context;
            _navigationService = navigationService;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void OnNavigatedTo(object? parameter)
        {
            if (parameter is int contactId && contactId > 0)
            {
                _contact = _context.Contacts.Find(contactId);
                _isNewContact = false;
            }
            else
            {
                _contact = new Contact();
                _isNewContact = true;
            }

            OnPropertyChanged(nameof(EditName));
            OnPropertyChanged(nameof(EditPhone));
        }

        private void Save()
        {
            if (_contact == null) return;


            try
            {
                if (_isNewContact)
                {
                    _context.Contacts.Add(_contact);
                }
                else if (_context.Entry(_contact).State == EntityState.Detached)
                {
                    _context.Contacts.Update(_contact);
                }

                _context.SaveChanges();

                _navigationService.NavigateTo<ContactsListViewModel>();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            if (!_isNewContact && _contact != null)
            {
                var entry = _context.Entry(_contact);
                if (entry.State != EntityState.Detached)
                {
                    entry.Reload();
                }
            }

            _navigationService.NavigateTo<ContactsListViewModel>();
        }
    }
}