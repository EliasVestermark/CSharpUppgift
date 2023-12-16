using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Contact = ClassLibrary.Models.Contact;

namespace AddressBookMaui.ViewModels;

public partial class ContactListViewModel : ObservableObject
{
    private readonly IContactService _contactService;

    public ContactListViewModel(IContactService contactService)
    {
        _contactService = contactService;
        GetContactList();
    }

    [ObservableProperty]
    private string _statusMessage = null!;

    [ObservableProperty]
    private ObservableCollection<IContact> _contactList = [];

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

    [RelayCommand]
    private async Task NavigateToUpdateContact(Contact contact)
    {
        await Shell.Current.GoToAsync($"UpdateContactPage", new Dictionary<string, object>
        {
            ["ContactDetails"] = contact
        });
    }

    [RelayCommand]
    private async Task NavigateToAddContact()
    {
        await Shell.Current.GoToAsync($"//AddContactPage");
    }

    private void AddIndexToContact()
    {
        int i = 1;

        foreach (Contact contact in ContactList)
        {
            contact.Index = i.ToString();
            i++;
        }
    }

    private void UpdateContactList()
    {
        ContactList = new ObservableCollection<IContact>(_contactService.Contacts.ToList());
    }
}
