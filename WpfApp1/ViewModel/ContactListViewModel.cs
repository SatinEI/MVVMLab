using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModel
{
    public class ContactsListViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        private readonly PhoneBookDbSatinEi2307b2Context _context;

        public ObservableCollection<Contact> Contacts { get; }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (Set(ref _name, value))
                    CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _phone = string.Empty;
        public string Phone
        {
            get => _phone;
            set
            {
                if (Set(ref _phone, value))
                    CommandManager.InvalidateRequerySuggested();
            }
        }

        private Contact? _selectedContact;
        public Contact? SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (Set(ref _selectedContact, value))
                    CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public ContactsListViewModel(
            PhoneBookDbSatinEi2307b2Context context,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            _context = context;
            _dialogService = dialogService;
            _navigationService = navigationService;

            Contacts = new ObservableCollection<Contact>(_context.Contacts.ToList());

            AddCommand = new RelayCommand(AddContact, CanAddContact);
            DeleteCommand = new RelayCommand(DeleteContact, CanDeleteContact);
            EditCommand = new RelayCommand(EditContact, () => SelectedContact != null);
        }

        private void AddContact()
        {
            if (Contacts.Any(c => c.Phone == Phone))
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует!");
                return;
            }

            try
            {
                var newContact = new Contact { Name = Name, Phone = Phone };

                _context.Contacts.Add(newContact);

                _context.SaveChanges();

                Contacts.Add(newContact);

                _dialogService.ShowInfo($"Контакт \"{newContact.Name}\" успешно добавлен.");

                // Очищаем поля ввода
                Name = string.Empty;
                Phone = string.Empty;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка при добавлении контакта: {ex.Message}");
            }
        }

        private bool CanAddContact()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            if (string.IsNullOrWhiteSpace(Phone))
                return false;

            var regex = new System.Text.RegularExpressions.Regex(@"^(\+7)?\d{10}$");
            return regex.IsMatch(Phone);
        }
        private void DeleteContact()
        {
            if (SelectedContact == null) return;

            if (_dialogService.ShowConfirmation(
                $"Удалить контакт \"{SelectedContact.Name}\"?"))
            {
                try
                {
                    _context.Contacts.Remove(SelectedContact);

                    _context.SaveChanges();

                    Contacts.Remove(SelectedContact);
                    SelectedContact = null;
                }
                catch (Exception ex)
                {
                    _dialogService.ShowError($"Ошибка при удалении контакта: {ex.Message}");
                }
            }
        }

        private bool CanDeleteContact() => SelectedContact != null;
        private void EditContact()
        {
            if (SelectedContact != null)
                _navigationService.NavigateTo<ContactEditViewModel>(SelectedContact.Id);
        }
    }
}