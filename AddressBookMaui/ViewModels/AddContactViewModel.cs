
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Contact = ClassLibrary.Models.Contact;


namespace AddressBookMaui.ViewModels;

public partial class AddContactViewModel : ObservableObject
{
    /// <summary>
    /// A field, type IContactService,to be used to reach the instance of ContactService created by DI
    /// </summary>
    private readonly IContactService _contactService;

    /// <summary>
    /// Constructor that takes a param then saves the value of the param to the field _contactService
    /// </summary>
    /// <param name="contactService">IContactService</param>
    public AddContactViewModel(IContactService contactService)
    {
        _contactService = contactService;
    }

    [ObservableProperty]
    private Contact _addContactForm = new();

    [ObservableProperty]
    private string _statusMessage = null!;

    [ObservableProperty]
    private ObservableCollection<IContact> _contactList = [];

    /// <summary>
    /// Adds a contact to list throught AddContact, simple required field validation as well as digit validation if phone number or postal code are entered
    /// </summary>
    [RelayCommand]
    public void AddContactToList()
    {
        if (!_contactService.NumberValidation(AddContactForm!.PhoneNumber) || !_contactService.NumberValidation(AddContactForm.PostalCode))
        {
            StatusMessage = "Please provide a valid phone number and/or postal code";
        }
        else if (AddContactForm != null && !string.IsNullOrWhiteSpace(AddContactForm.FirstName) && !string.IsNullOrWhiteSpace(AddContactForm.LastName) && !string.IsNullOrWhiteSpace(AddContactForm.Email))
        {
            var result = _contactService.AddContact(AddContactForm);

            switch (result.Status)
            {
                case ClassLibrary.Enums.ServiceStatus.SUCCESS:
                    UpdateContactsList();
                    StatusMessage = "The contact has been added successfully";
                    ResetStatusMessage();
                    AddContactForm = new();
                    break;

                case ClassLibrary.Enums.ServiceStatus.ALREADY_EXISTS:
                    StatusMessage = "A contact with the same email already exists";
                    AddContactForm = new();
                    break;

                case ClassLibrary.Enums.ServiceStatus.FAILED:
                    StatusMessage = "An error occured when trying to add the contact, please try again";
                    AddContactForm = new();
                    break;
            }
        }
        else
        {
            StatusMessage = "Please provide a full name as well as an email then try again";
        }
    }

    /// <summary>
    /// Navigates to ContactListPage
    /// </summary>
    [RelayCommand]
    private async Task NavigateToList()
    {
        await Shell.Current.GoToAsync("ContactListPage");
    }

    /// <summary>
    /// Two seconds delay before the status message is "removed" by using an empty string
    /// </summary>
    private async void ResetStatusMessage()
    {
        await Task.Delay(2000);
        StatusMessage = "";
    }

    /// <summary>
    /// Updates the Observable property list with the list from the jsonfile
    /// </summary>
    private void UpdateContactsList()
    {
        ContactList = new ObservableCollection<IContact>(_contactService.Contacts.ToList());
    }
}
