using AddressBookMaui.ViewModels;

namespace AddressBookMaui.Views;

public partial class AddContactPage : ContentPage
{
    /// <summary>
    /// Binds the viewmodel to the corresponding page
    /// </summary>
    /// <param name="viewModel">AddContactViewModel</param>
    public AddContactPage(AddContactViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}