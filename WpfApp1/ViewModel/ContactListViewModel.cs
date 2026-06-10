using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModel
{
    public class ContactsListViewModel : ObservableObject
    {
        private readonly IDbContextFactory<PhoneBookDbSatinEi2307b2Context> _factory;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<Contact> Contacts { get; } = new();

        private string _name = string.Empty;
        public string Name { get => _name; set { if (Set(ref _name, value)) CommandManager.InvalidateRequerySuggested(); } }

        private string _phone = string.Empty;
        public string Phone { get => _phone; set { if (Set(ref _phone, value)) CommandManager.InvalidateRequerySuggested(); } }

        private Contact? _selectedContact;
        public Contact? SelectedContact { get => _selectedContact; set { if (Set(ref _selectedContact, value)) CommandManager.InvalidateRequerySuggested(); } }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public ContactsListViewModel(IDbContextFactory<PhoneBookDbSatinEi2307b2Context> factory,
                                     IDialogService dialogService,
                                     INavigationService navigationService)
        {
            _factory = factory;
            _dialogService = dialogService;
            _navigationService = navigationService;

            AddCommand = new RelayCommand(AddContact, CanAddContact);
            DeleteCommand = new RelayCommand(DeleteContact, CanDeleteContact);
            EditCommand = new RelayCommand(EditContact, () => SelectedContact != null);

            LoadContacts(); 
        }

        private void LoadContacts()
        {
            Contacts.Clear();
            using var context = _factory.CreateDbContext();
            var items = context.Contacts.ToList();
            foreach (var c in items)
                Contacts.Add(c);
        }

        private void AddContact()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Phone))
            {
                _dialogService.ShowWarning("Заполните имя и телефон.");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(Phone, @"^(\+7)?\d{10}$"))
            {
                _dialogService.ShowWarning("Телефон должен быть в формате +7XXXXXXXXXX или XXXXXXXXXX.");
                return;
            }

            using var context = _factory.CreateDbContext();
            var newContact = new Contact { Name = Name, Phone = Phone };
            context.Contacts.Add(newContact);
            context.SaveChanges();

            Contacts.Add(newContact);
            _dialogService.ShowInfo($"Контакт \"{newContact.Name}\" добавлен.");
            Name = string.Empty;
            Phone = string.Empty;
        }

        private bool CanAddContact() => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);

        private void DeleteContact()
        {
            if (SelectedContact == null) return;
            if (!_dialogService.ShowConfirmation($"Удалить контакт \"{SelectedContact.Name}\"?"))
                return;

            using var context = _factory.CreateDbContext();
            var contactToDelete = context.Contacts.Find(SelectedContact.Id);
            if (contactToDelete != null)
            {
                context.Contacts.Remove(contactToDelete);
                context.SaveChanges();
            }

            Contacts.Remove(SelectedContact);
            SelectedContact = null;
        }

        private bool CanDeleteContact() => SelectedContact != null;

        private void EditContact()
        {
            if (SelectedContact != null)
                _navigationService.NavigateTo<ContactEditViewModel>(SelectedContact.Id);
        }
    }
}