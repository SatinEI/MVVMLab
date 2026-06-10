using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModel
{
    public class ContactEditViewModel : ObservableObject, INavigationAware
    {
        private readonly IDbContextFactory<PhoneBookDbSatinEi2307b2Context> _factory;
        private readonly INavigationService _navigationService;

        private int _editingContactId;
        private string _editName = string.Empty;
        private string _editPhone = string.Empty;

        public string EditName
        {
            get => _editName;
            set { if (Set(ref _editName, value)) CommandManager.InvalidateRequerySuggested(); }
        }

        public string EditPhone
        {
            get => _editPhone;
            set { if (Set(ref _editPhone, value)) CommandManager.InvalidateRequerySuggested(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ContactEditViewModel(IDbContextFactory<PhoneBookDbSatinEi2307b2Context> factory,
                                    INavigationService navigationService)
        {
            _factory = factory;
            _navigationService = navigationService;
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void OnNavigatedTo(object? parameter)
        {
            if (parameter is int contactId && contactId > 0)
            {
                _editingContactId = contactId;
                using var context = _factory.CreateDbContext();
                var contact = context.Contacts.Find(contactId);
                if (contact != null)
                {
                    EditName = contact.Name;
                    EditPhone = contact.Phone;
                }
            }
            else
            {
                _editingContactId = 0;
                EditName = string.Empty;
                EditPhone = string.Empty;
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(EditName) && !string.IsNullOrWhiteSpace(EditPhone);

        private void Save()
        {
            if (!CanSave()) return;

            if (!System.Text.RegularExpressions.Regex.IsMatch(EditPhone, @"^(\+7)?\d{10}$"))
            {
                System.Windows.MessageBox.Show("Неверный формат телефона.", "Ошибка");
                return;
            }

            using var context = _factory.CreateDbContext();

            if (_editingContactId == 0)
            {
                var newContact = new Contact { Name = EditName, Phone = EditPhone };
                context.Contacts.Add(newContact);
                context.SaveChanges();
            }
            else
            {
                var contactToUpdate = context.Contacts.Find(_editingContactId);
                if (contactToUpdate != null)
                {
                    contactToUpdate.Name = EditName;
                    contactToUpdate.Phone = EditPhone;
                    context.SaveChanges();
                }
            }

            _navigationService.NavigateTo<ContactsListViewModel>();
        }

        private void Cancel()
        {
            _navigationService.NavigateTo<ContactsListViewModel>();
        }
    }
}