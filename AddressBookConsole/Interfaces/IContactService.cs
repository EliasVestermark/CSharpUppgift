using ClassLibrary.Interfaces;
using ClassLibrary.Models;

namespace AddressBookConsole.Interfaces;

public interface IContactService
{
    IServiceResult AddContact(Contact contact);
    IServiceResult RemoveContact(string id);
    IServiceResult UpdateContact(string email, string newFirstName, string newLastName, int newPhoneNumber, string newEmail, string newAddress, int newPostalCode, string newCity);
    IServiceResult GetContactInformation(int option);
    IServiceResult GetAllContacts();
    List<Contact> GetDataFromJSONFile();
}
