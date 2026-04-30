using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
        /// <summary>
        /// Абстракция для отображения диалоговых окон.
        /// Позволяет ViewModel не зависеть от конкретной реализации UI.
        /// </summary>
        public interface IDialogService
        {
            void ShowInfo(string message, string title = "Информация");
            void ShowWarning(string message, string title = "Предупреждение");
            void ShowError(string message, string title = "Ошибка");
            /// <summary>
            /// Запрос подтверждения (Да/Нет). Возвращает true, если пользователь выбрал "Да".
            /// </summary>
            bool ShowConfirmation(string message, string title = "Подтверждение");
        }
}
