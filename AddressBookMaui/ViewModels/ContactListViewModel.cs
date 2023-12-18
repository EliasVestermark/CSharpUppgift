using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Contact = ClassLibrary.Models.Contact;

namespace AddressBookMaui.ViewModels;

public partial class ContactListViewModel : ObservableObject
{
    /// <summary>
    /// A field, type IContactService,to be used to reach the instance of ContactService created by DI
    /// </summary>
    private readonly IContactService _contactService;

    /// <summary>
    /// Constructor that takes a param then saves the value of the param to the field _contactService.
    /// GetContactList on initilization to load the list with the correct information
    /// </summary>
    /// <param name="contactService">IContactService</param>
    public ContactListViewModel(IContactService contactService)
    {
        _contactService = contactService;

        _contactService.ContactsUpdated += (sender, e) =>
        {
            var result = _contactService.GetAllContacts();

            if (result.Result is List<IContact> contactList)
            {
                ContactList = new ObservableCollection<IContact>(contactList.ToList());
            }
        };
        GetContactList();
    }

    [ObservableProperty]
    private string _statusMessage = null!;

    [ObservableProperty]
    private ObservableCollection<IContact> _contactList = [];

    /// <summary>
    /// Method that is called by pressing a button in the program. Uses GetAllContacts to get the list of contacts. 
    /// Stores the list in an observable property (ContactList) or shows a status message depending on the result.Status.
    /// </summary>
    public void GetContactList()
    {
        var result = _contactService.GetAllContacts();

        switch (result.Status)
        {
            case ClassLibrary.Enums.ServiceStatus.SUCCESS:
                if (result.Result is List<IContact> contactList)
                {
                    ContactList = new ObservableCollection<IContact>(contactList.ToList());
                    AddIndexToContact();
                }
                break;

            case ClassLibrary.Enums.ServiceStatus.NOT_FOUND:
                StatusMessage = "The list is empty";
                break;

            case ClassLibrary.Enums.ServiceStatus.FAILED:
                StatusMessage = "An error occurred when trying to get the list";
                break;
        }
    }

    /// <summary>
    /// Navigates to UpdateContactPage, passes an object parameter as a queryPorperty
    /// </summary>
    [RelayCommand]
    private async Task NavigateToUpdateContact(Contact contact)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            ["ContactDetails"] = contact
        };

        await Shell.Current.GoToAsync($"UpdateContactPage", parameters);
    }

    /// <summary>
    /// Navigates to root AddContactPage
    /// </summary>
    [RelayCommand]
    private async Task NavigateToAddContact()
    {
        await Shell.Current.GoToAsync($"//AddContactPage");
    }

    /// <summary>
    /// Dynamically adds an index value to each contact in the list when the list is updated
    /// </summary>
    private void AddIndexToContact()
    {
        int i = 1;

        foreach (Contact contact in ContactList)
        {
            contact.Index = i.ToString();
            i++;
        }
    }

    /// <summary>
    /// Updates the Observable property list with the list from the jsonfile
    /// </summary>
    private void UpdateContactList()
    {
        ContactList = new ObservableCollection<IContact>(_contactService.Contacts.ToList());
    }
}
