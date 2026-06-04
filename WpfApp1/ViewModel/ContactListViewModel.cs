using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModel
{
    public class ContactListViewModel : ObservableObject
    {
        private readonly PhoneBookDbSatinEi2307b2Context _context;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<Contact> Contacts { get; }

        public ContactListViewModel(
            PhoneBookDbSatinEi2307b2Context context,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            _context = context;
            _dialogService = dialogService;
            _navigationService = navigationService;

            Contacts = new ObservableCollection<Contact>(_context.Contacts.ToList());
        }
    }
}