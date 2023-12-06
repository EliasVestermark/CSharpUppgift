
using AddressBookConsole.Interfaces;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.Models.Responses;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ClassLibrary.Services;

public class ContactService : IContactService
{
    private readonly IFileService _fileService;

    public ContactService(IFileService fileService)
    {
        _fileService = fileService;
    }

    private List<IContact> _contacts = [];

    public IServiceResult AddContact(IContact contact)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            var content = _fileService.GetContentFromFile();
            _contacts = JsonConvert.DeserializeObject<List<Contact>>(content)!;

            if (!_contacts.Any(x => x.Email == contact.Email))
            {

                _contacts.Add(contact);
                _fileService.SaveContentToFile(JsonConvert.SerializeObject(_contacts));
                response.Status = Enums.ServiceStatus.SUCCESS;
                return response;
            }
            else
            {
                response.Status = Enums.ServiceStatus.ALREADY_EXISTS;
                return response;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILED;
            return response;
        }
    }

    public IServiceResult GetAllContacts()
    {
        IServiceResult response = new ServiceResult();
        IEnumerable<IContact> iEnumerableContacts = [];

        try
        {
            var content = _fileService.GetContentFromFile();

            if (!string.IsNullOrEmpty(content))
            {
                List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(content)!;
                iEnumerableContacts = contacts.Cast<IContact>()!;

                response.Status = Enums.ServiceStatus.SUCCESS;
                response.Result = iEnumerableContacts;
                return response;
            }

            response.Status = Enums.ServiceStatus.SUCCESS;
            response.Result = iEnumerableContacts;
            return response;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILED;
            return response;
        }
    }

    public IServiceResult GetContactInformation(int option)
    {
        var content = _fileService.GetContentFromFile();

        IServiceResult response = new ServiceResult();
        try
        {
            List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(content)!;

            response.Status = Enums.ServiceStatus.SUCCESS;
            response.Result = contacts[option - 1];
            return response;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILED;
            return response;
        }
    }

    public IServiceResult RemoveContact(string email) 
    {
        IServiceResult response = new ServiceResult();
        try
        {
            var employeeToRemove = _contacts.FirstOrDefault(x => x.Email == email);

            if (employeeToRemove == null)
            {
                response.Status = Enums.ServiceStatus.NOT_FOUND;
                return response;
            }
            else
            {
                _contacts.Remove(employeeToRemove);
                _fileService.SaveContentToFile(JsonConvert.SerializeObject(_contacts));
                response.Status = Enums.ServiceStatus.SUCCESS;
                return response;
            }
        }
        catch
        {
            response.Status = Enums.ServiceStatus.FAILED;
            return response;
        }
    }

    public IServiceResult UpdateContact(string id, string newName, string newPosition)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            var employeeToUpdate = _contacts.FirstOrDefault(x => x.Email == id);

            if (employeeToUpdate == null)
            {
                response.Status = Enums.ServiceStatus.NOT_FOUND;
                return response;
            }
            else
            {
                //employeeToUpdate.Name = newName;
                //employeeToUpdate.Position = newPosition;
                //response.Status = Enums.ServiceStatus.SUCCESS;
                return response;
            }
        }
        catch
        {
            response.Status = Enums.ServiceStatus.FAILED;
            return response;
        }
    }
}
