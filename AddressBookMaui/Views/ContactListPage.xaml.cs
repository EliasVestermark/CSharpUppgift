using AddressBookMaui.ViewModels;

namespace AddressBookMaui.Views;

public partial class ContactListPage : ContentPage
{
    /// <summary>
    /// Binds the viewmodel to the corresponding page
    /// </summary>
    /// <param name="viewModel">ContactListViewModel</param>
    public ContactListPage(ContactListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}