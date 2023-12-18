using ClassLibrary.Models;

namespace ClassLibrary.Interfaces;

public interface IContactService
{
    List<IContact> Contacts { get; }

    event EventHandler? ContactsUpdated;

    IServiceResult AddContact(IContact contact);
    IServiceResult RemoveContact(string id);
    IServiceResult UpdateContact(string email, string newFirstName, string newLastName, string newPhoneNumber, string newEmail, string newAddress, string newPostalCode, string newCity);
    IServiceResult GetContactInformation(int option);
    IServiceResult GetAllContacts();
    List<IContact> GetDataFromJSONFile();
    bool NumberValidation(string number);
}
