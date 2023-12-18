using AddressBookMaui.Views;

namespace AddressBookMaui
{
    public partial class AppShell : Shell
    {
        /// <summary>
        /// Resgisters the routing routes for each page, used for shell navigation
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ContactListPage), typeof(ContactListPage));
            Routing.RegisterRoute(nameof(UpdateContactPage), typeof(UpdateContactPage));
            Routing.RegisterRoute(nameof(AddContactPage), typeof(AddContactPage));
        }
    }
}
