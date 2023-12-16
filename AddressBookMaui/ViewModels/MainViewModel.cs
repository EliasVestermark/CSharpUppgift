
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Contact = ClassLibrary.Models.Contact;

namespace AddressBookMaui.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IContactService _contactService;

    public MainViewModel(IContactService contactService)
    {
        _contactService = contactService;
    }

    [ObservableProperty]
    private IContact _contactForm = new Contact();

    [ObservableProperty]
    private string _statusMessage = null!;

    [ObservableProperty]
    private ObservableCollection<IContact> _contactList = [];

    [RelayCommand]
    public void AddContactToList()
    {
        if (!_contactService.NumberValidation(ContactForm!.PhoneNumber) || !_contactService.NumberValidation(ContactForm.PostalCode))
        {
            StatusMessage = "Please provide a valid phone number and/or postal code";
        }
        else if (ContactForm != null && !string.IsNullOrWhiteSpace(ContactForm.FirstName) && !string.IsNullOrWhiteSpace(ContactForm.LastName) && !string.IsNullOrWhiteSpace(ContactForm.Email))
        {
            var result = _contactService.AddContact(ContactForm);

            switch (result.Status)
            {
                case ClassLibrary.Enums.ServiceStatus.SUCCESS:
                    UpdateContactsList();
                    StatusMessage = "The contact has been added successfully";
                    ContactForm = new Contact();
                    break;

                case ClassLibrary.Enums.ServiceStatus.ALREADY_EXISTS:
                    StatusMessage = "A contact with the same email already exists";
                    ContactForm = new Contact();
                    break;

                case ClassLibrary.Enums.ServiceStatus.FAILED:
                    StatusMessage = "An error occured when trying to add the contact, please try again";
                    ContactForm = new Contact();
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

    private void UpdateContactsList()
    {
        ContactList = new ObservableCollection<IContact>(_contactService.Contacts.ToList());
    }
}
