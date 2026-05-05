using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    /// <summary>
    /// Интерфейс для ViewModel, которые могут принимать параметры при навигации.
    /// </summary>
    public interface INavigationAware
    {
        void OnNavigatedTo(object? parameter);
    }
}