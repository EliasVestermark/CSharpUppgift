using AddressBookMaui.ViewModels;

namespace AddressBookMaui.Views;

public partial class AddContactPage : ContentPage
{
	public AddContactPage(AddContactViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}