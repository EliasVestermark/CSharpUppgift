using AddressBookMaui.Views;

namespace AddressBookMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ContactListPage), typeof(ContactListPage));
            Routing.RegisterRoute(nameof(UpdateContactPage), typeof(UpdateContactPage));
            Routing.RegisterRoute(nameof(AddContactPage), typeof(AddContactPage));
        }
    }
}
