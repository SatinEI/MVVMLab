using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Model.PhoneBook.Models;

namespace WpfApp1.ViewModel
{
    /// <summary>
    /// Главная модель представления.
    /// Посредник между View (окном) и Model (Contact).
    /// Предоставляет данные, состояния и команды для взаимодействия с UI.
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        // Свойство для хранения списка контактов.
        // ObservableCollection автоматически оповещает UI об изменениях (добавление/удаление).
        public ObservableCollection<Contact> Contacts { get; }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);   // Уведомление View об изменении
        }

        private string _phone = string.Empty;
        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
        }

        private Contact? _selectedContact;
        public Contact? SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        // Команды, привязанные к кнопкам в View
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            Contacts = new ObservableCollection<Contact>();

            // Команда добавления: выполняет AddContact, доступность определяется CanAddContact
            AddCommand = new RelayCommand(AddContact, CanAddContact);

            // Команда удаления: выполняет DeleteContact, доступность – CanDeleteContact
            DeleteCommand = new RelayCommand(DeleteContact, CanDeleteContact);
        }

        /// <summary>
        /// Добавляет новый контакт в коллекцию.
        /// После добавления очищает поля ввода.
        /// </summary>
        private void AddContact()
        {
            var newContact = new Contact(Name, Phone);
            Contacts.Add(newContact);

            // Очистка полей ввода
            Name = string.Empty;
            Phone = string.Empty;
        }

        /// <summary>
        /// Проверяет, можно ли добавить контакт.
        /// Имя и телефон должны быть заполнены и проходить валидацию.
        /// </summary>
        private bool CanAddContact()
        {
            // Используем временный объект для проверки, не модифицируя состояние
            var tempContact = new Contact(Name, Phone);
            return tempContact.Validate();
        }

        /// <summary>
        /// Удаляет выбранный контакт из коллекции.
        /// </summary>
        private void DeleteContact()
        {
            if (SelectedContact != null)
            {
                Contacts.Remove(SelectedContact);
                // После удаления сбрасываем выбор
                SelectedContact = null;
            }
        }

        /// <summary>
        /// Проверяет, можно ли удалить контакт (выбран ли какой-либо).
        /// </summary>
        private bool CanDeleteContact()
        {
            return SelectedContact != null;
        }
    }
}
