
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Contact = ClassLibrary.Models.Contact;


namespace AddressBookMaui.ViewModels;

public partial class AddContactViewModel : ObservableObject
{
    private readonly IContactService _contactService;

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

    [RelayCommand]
    private async Task NavigateToList()
    {
        await Shell.Current.GoToAsync("ContactListPage");
    }

    private async void ResetStatusMessage()
    {
        await Task.Delay(2000);
        StatusMessage = "";
    }

    private void UpdateContactsList()
    {
        ContactList = new ObservableCollection<IContact>(_contactService.Contacts.ToList());
    }
}
