using AddressBookMaui.ViewModels;

namespace AddressBookMaui.Views;

public partial class ContactListPage : ContentPage
{
	public ContactListPage(ContactListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}