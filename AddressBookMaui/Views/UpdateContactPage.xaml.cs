using AddressBookMaui.ViewModels;

namespace AddressBookMaui.Views;

public partial class UpdateContactPage : ContentPage
{
    /// <summary>
    /// Binds the viewmodel to the corresponding page
    /// </summary>
    /// <param name="viewModel">UpdateContactViewModel</param>
    public UpdateContactPage(UpdateContactViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}