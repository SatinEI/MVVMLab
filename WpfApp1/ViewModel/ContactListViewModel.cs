using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using WpfApp1.Model;
using WpfApp1.Services;

namespace WpfApp1.ViewModel
{
    public class ContactListViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<Contact> Contacts { get; }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set { if (Set(ref _name, value)) CommandManager.InvalidateRequerySuggested(); }
        }

        private string _phone = string.Empty;
        public string Phone
        {
            get => _phone;
            set { if (Set(ref _phone, value)) CommandManager.InvalidateRequerySuggested(); }
        }

        private Contact? _selectedContact;
        public Contact? SelectedContact
        {
            get => _selectedContact;
            set { if (Set(ref _selectedContact, value)) CommandManager.InvalidateRequerySuggested(); }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }   // новая команда

        public ContactListViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            Contacts = new ObservableCollection<Contact>();

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
            var newContact = new Contact(Name, Phone);
            Contacts.Add(newContact);
            _dialogService.ShowInfo($"Контакт \"{newContact.Name}\" успешно добавлен.");
            Name = string.Empty;
            Phone = string.Empty;
        }

        private bool CanAddContact() => new Contact(Name, Phone).Validate();

        private void DeleteContact()
        {
            if (SelectedContact == null) return;
            if (_dialogService.ShowConfirmation($"Удалить контакт \"{SelectedContact.Name}\"?"))
            {
                Contacts.Remove(SelectedContact);
                SelectedContact = null;
            }
        }

        private bool CanDeleteContact() => SelectedContact != null;

        private void EditContact()
        {
            if (SelectedContact != null)
                _navigationService.NavigateTo<ContactEditViewModel>(SelectedContact);
        }
    }
}