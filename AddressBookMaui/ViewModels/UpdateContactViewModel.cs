
using ClassLibrary.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Diagnostics;
using Contact = ClassLibrary.Models.Contact;

namespace AddressBookMaui.ViewModels;

[QueryProperty(nameof(ContactDetails), nameof(ContactDetails))]

public partial class UpdateContactViewModel : ObservableObject
{
    private readonly IContactService _contactService;

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

    [RelayCommand]
    private void UpdateContact()
    {
        var result = _contactService.UpdateContact(ContactDetails.Email, UpdatedContact.FirstName, UpdatedContact.LastName, UpdatedContact.PhoneNumber, UpdatedContact.Email, UpdatedContact.Address, UpdatedContact.PostalCode, UpdatedContact.City);

        switch (result.Status)
        {
            case ClassLibrary.Enums.ServiceStatus.SUCCESS:
                ContactDetails = UpdatedContact;
                UpdatedContact = new();
                break;

            case ClassLibrary.Enums.ServiceStatus.NOT_FOUND:
                StatusMessage = "The contact could not be found";
                break;

            case ClassLibrary.Enums.ServiceStatus.FAILED:
                StatusMessage = "Ann error occurred when trying to update the contact";
                break;
        }
    }

    [RelayCommand]
    private void RemoveContactFromList(Contact contact)
    {
        if (contact.Email == EntryEmail)
        {
            var result = _contactService.RemoveContact(contact.Email);

            switch (result.Status)
            {
                case ClassLibrary.Enums.ServiceStatus.SUCCESS:
                    EntryEmail = "";
                    NavigateToList();
                    break;

                case ClassLibrary.Enums.ServiceStatus.NOT_FOUND:
                    StatusMessage = "The contact could not be found";
                    break;

                case ClassLibrary.Enums.ServiceStatus.FAILED:
                    StatusMessage = "Ann error occurred when trying to delete the contact";
                    break;
            }
        }
        else
        {
            StatusMessage = "The email doesn't match, try again";
        }
    }

    [RelayCommand]
    private async Task NavigateToAddContact()
    {
        await Shell.Current.GoToAsync("//AddContactPage");
    }

    [RelayCommand]
    private async Task NavigateToList()
    {
        await Shell.Current.GoToAsync("ContactListPage");
    }
}
