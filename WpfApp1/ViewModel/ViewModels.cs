using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
        public class ViewModels : ObservableObject
        {
            private readonly IDialogService _dialogService;

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

            /// <summary>
            /// Конструктор с внедрением зависимости IDialogService.
            /// </summary>
            public ViewModels(IDialogService dialogService)
            {
                _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

                Contacts = new ObservableCollection<Contact>();

                AddCommand = new RelayCommand(AddContact, CanAddContact);
                DeleteCommand = new RelayCommand(DeleteContact, CanDeleteContact);
            }

            private void AddContact()
            {
                // Проверка дубликата по номеру телефона
                if (Contacts.Any(c => c.Phone == Phone))
                {
                    _dialogService.ShowWarning("Контакт с таким номером уже существует!");
                    return;
                }

                var newContact = new Contact(Name, Phone);
                Contacts.Add(newContact);

                _dialogService.ShowInfo($"Контакт \"{newContact.Name}\" успешно добавлен.");

                // Очистка полей ввода
                Name = string.Empty;
                Phone = string.Empty;
            }

            private bool CanAddContact()
            {
                var temp = new Contact(Name, Phone);
                return temp.Validate();
            }

            private void DeleteContact()
            {
                if (SelectedContact == null) return;

                bool confirmed = _dialogService.ShowConfirmation(
                    $"Удалить контакт \"{SelectedContact.Name}\"?",
                    "Подтверждение удаления");

                if (confirmed)
                {
                    Contacts.Remove(SelectedContact);
                    SelectedContact = null;
                }
            }

            private bool CanDeleteContact() => SelectedContact != null;
        }
    }
