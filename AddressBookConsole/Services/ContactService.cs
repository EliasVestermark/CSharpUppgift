
using AddressBookConsole.Interfaces;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.Models.Responses;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace ClassLibrary.Services;

public class ContactService : IContactService
{
    private readonly IFileService _fileService;

    public ContactService(IFileService fileService)
    {
        _fileService = fileService;
    }

    private readonly List<IContact> _contacts = [];

    public IServiceResult AddContact(IContact contact)
    {
        IServiceResult response = new ServiceResult();
        try
        {
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
        try
        {
            response.Status = Enums.ServiceStatus.SUCCESS;
            response.Result = _contacts;
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
        IServiceResult response = new ServiceResult();
        try
        {
            response.Status = Enums.ServiceStatus.SUCCESS;
            response.Result = _contacts[option - 1];
            return response;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILED;
            return response;
        }
    }

    public IServiceResult RemoveContact(string id)
    {
        throw new NotImplementedException();
    }

    public IServiceResult UpdateContact(string id, string newName, string newPosition)
    {
        throw new NotImplementedException();
    }
}
