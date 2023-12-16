using AddressBookMaui.ViewModels;

namespace AddressBookMaui.Views;

public partial class UpdateContactPage : ContentPage
{
    public UpdateContactPage(UpdateContactViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}