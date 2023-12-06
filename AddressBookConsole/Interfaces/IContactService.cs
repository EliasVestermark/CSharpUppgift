using ClassLibrary.Interfaces;
using ClassLibrary.Models;

namespace AddressBookConsole.Interfaces;

public interface IContactService
{
    IServiceResult AddContact(IContact contact);
    IServiceResult RemoveContact(string id);
    IServiceResult UpdateContact(string id, string newName, string newPosition);
    IServiceResult GetContactInformation(int option);
    IServiceResult GetAllContacts();
}
