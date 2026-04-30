using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using WpfApp1.ViewModel;

namespace WpfApp1.Model
{


        /// <summary>
        /// Модель данных контакта.
        /// Содержит бизнес-данные (имя, телефон) и валидацию.
        /// Наследуется от ObservableObject для уведомления ViewModel об изменениях.
        /// </summary>
        public class Contact : ObservableObject
        {
            private string _name = string.Empty;
            private string _phone = string.Empty;

            /// <summary>
            /// Конструктор с проверкой корректности переданных значений.
            /// </summary>
            public Contact(string name, string phone)
            {
                Name = name;
                Phone = phone;
            }

            public string Name
            {
                get => _name;
                set
                {
                    if (Set(ref _name, value))      // Set вернёт true, если значение изменилось
                        OnPropertyChanged(nameof(Name));
                }
            }

            public string Phone
            {
                get => _phone;
                set
                {
                    if (Set(ref _phone, value))
                        OnPropertyChanged(nameof(Phone));
                }
            }

            public bool Validate()
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return false;

                if (string.IsNullOrWhiteSpace(Phone))
                    return false;

                if (Phone.StartsWith("+7"))
                {
                    return Phone.Length == 12 && Phone.Substring(2).All(char.IsDigit);
                }
                else
                {
                    return Phone.Length == 10 && Phone.All(char.IsDigit);
                }
            }
        }
    }
