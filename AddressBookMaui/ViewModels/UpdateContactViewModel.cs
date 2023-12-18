
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contact = ClassLibrary.Models.Contact;

namespace AddressBookMaui.ViewModels;

/// <summary>
/// QueryProperty, contains an object and is passed during navigation. The object is stored in the IContact ContactDetails
/// </summary>
//[QueryProperty(nameof(ContactDetails), nameof(ContactDetails))]

public partial class UpdateContactViewModel : ObservableObject, IQueryAttributable
{
    /// <summary>
    /// A field, type IContactService,to be used to reach the instance of ContactService created by DI
    /// </summary>
    private readonly IContactService _contactService;

    /// <summary>
    /// Constructor that takes a param then saves the value of the param to the field _contactService
    /// </summary>
    /// <param name="contactService">IContactService</param>
    public UpdateContactViewModel(IContactService contactService)
    {
        _contactService = contactService;
    }

    [ObservableProperty]
    private Contact _contactDetails = null!;

    [ObservableProperty]
    private Contact _updatedContact = new();

    [ObservableProperty]
    private string _entryEmail = null!;

    [ObservableProperty]
    private string _statusMessage = null!;

    /// <summary>
    /// Method that is called by pressing a button in the program. Uses UpdateContact to update by providing with the user input values. 
    /// Updates the visible information or shows a status message depending on the result.Status.
    /// </summary>
    [RelayCommand]
    private void UpdateContact()
    {
        var result = _contactService.UpdateContact(ContactDetails.Email, UpdatedContact.FirstName, UpdatedContact.LastName, UpdatedContact.PhoneNumber, UpdatedContact.Email, UpdatedContact.Address, UpdatedContact.PostalCode, UpdatedContact.City);

        switch (result.Status)
        {
            case ClassLibrary.Enums.ServiceStatus.SUCCESS:
                ContactDetails = UpdatedContact;
                UpdatedContact = new Contact();
                break;

            case ClassLibrary.Enums.ServiceStatus.NOT_FOUND:
                StatusMessage = "The contact could not be found";
                break;

            case ClassLibrary.Enums.ServiceStatus.FAILED:
                StatusMessage = "An error occurred when trying to update the contact";
                break;
        }
    }

    /// <summary>
    /// Method that is called by pressing a button in the program. Uses RemoveContact to remove the current contact provided that the email matches the user input. 
    /// Goes back to the contact list or shows a status message depending on the result.Status.
    /// </summary>
    /// <param name="contact">Contact to be removed, provided by CommandParameter</param>
    [RelayCommand]
    public void RemoveContactFromList(IContact contact)
    {
        if (contact.Email == EntryEmail)
        {
            var result = _contactService.RemoveContact(contact.Email);

            switch (result.Status)
            {
                case ClassLibrary.Enums.ServiceStatus.SUCCESS:
                    EntryEmail = string.Empty;
                    NavigateToList();
                    break;

                case ClassLibrary.Enums.ServiceStatus.NOT_FOUND:
                    StatusMessage = "The contact could not be found";
                    break;

                case ClassLibrary.Enums.ServiceStatus.FAILED:
                    StatusMessage = "An error occurred when trying to delete the contact";
                    break;
            }
        }
        else
        {
            StatusMessage = "The email doesn't match, try again";
        }
    }

    /// <summary>
    /// Navigates to root AddContactPage
    /// </summary>
    [RelayCommand]
    private async Task NavigateToAddContact()
    {
        await Shell.Current.GoToAsync("//AddContactPage");
    }

    /// <summary>
    /// Navigates to ContactListPage
    /// </summary>
    [RelayCommand]
    private async Task NavigateToList()
    {
        await Shell.Current.GoToAsync("ContactListPage");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ContactDetails = (query["ContactDetails"] as Contact)!;
    }
}
