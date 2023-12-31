﻿
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Models.Responses;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ClassLibrary.Services;

public class ContactService : IContactService
{
    /// <summary>
    /// A field, type IFileService,to be used to reach the instance of FileService created by DI
    /// </summary>
    private readonly IFileService _fileService;

    /// <summary>
    /// Constructor that takes a param then saves the value of the param to the field _fileService
    /// </summary>
    /// <param name="fileService">IFileService</param>
    public ContactService(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// List for the contacts
    /// </summary>
    public List<IContact> Contacts { get; private set; } = new List<IContact>();

    public event EventHandler? ContactsUpdated;

    /// <summary>
    /// Settings for json serializing
    /// </summary>
    private JsonSerializerSettings jsonSettings = new JsonSerializerSettings 
    {
        TypeNameHandling = TypeNameHandling.Objects
    };

    /// <summary>
    /// Adds contact to the list and writes it to the jsonfile
    /// </summary>
    /// <param name="contact">Object that holds the contact provided by the AdContactMenu</param>
    /// <returns>ServiceResult containing a ServiceStatus depending on the outcome</returns>
    public IServiceResult AddContact(IContact contact)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            GetDataFromJSONFile();

            //If-statement that makes sure no contact with the same email already exists
            if (!Contacts.Any(x => x.Email == contact.Email))
            {
                Contacts.Add(contact);
                //ContactsUpdated?.Invoke(this, new EventArgs());
                _fileService.SaveContentToFile(JsonConvert.SerializeObject(Contacts, jsonSettings));
                response.Status = ServiceStatus.SUCCESS;
                return response;
            }
            else
            {
                response.Status = ServiceStatus.ALREADY_EXISTS;
                return response;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            return response;
        }
    }

    /// <summary>
    /// Gets the listinformation from the jsonfile and stores it to _contacts
    /// </summary>
    /// <returns>ServiceResult containing a ServiceStatus depending on the outcome aswell as the _contacts list if successful</returns>
    public IServiceResult GetAllContacts()
    {
        IServiceResult response = new ServiceResult();

        try
        {
            var content = _fileService.GetContentFromFile();

            //Returns the list as a response.Result if not null or empty
            if (!string.IsNullOrEmpty(content))
            {
                Contacts = JsonConvert.DeserializeObject<List<IContact>>(content, jsonSettings)!;

                response.Status = ServiceStatus.SUCCESS;
                response.Result = Contacts;
                return response;
            }

            response.Status = ServiceStatus.NOT_FOUND;
            return response;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            return response;
        }
    }

    /// <summary>
    /// Gets contact information of a specified contact from the jsonfile depending on option-param
    /// </summary>
    /// <param name="option">contains user input of which contact they would like to see</param>
    /// <returns>ServiceResult containing a ServiceStatus depending on the outcome aswell as the correct contact from _contacts list if successful</returns>
    public IServiceResult GetContactInformation(int option)
    {
        IServiceResult response = new ServiceResult();

        try
        {
            GetDataFromJSONFile();
            response.Status = ServiceStatus.SUCCESS;
            // - 1 since the _contacts list starts at 0 but the viual list in the menu starts at 1
            response.Result = Contacts[option - 1];
            return response;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            return response;
        }
    }

    /// <summary>
    /// Gets the list from the jsonfile and removes the contact with the provided email
    /// </summary>
    /// <param name="email">The specified email of the contact to be removed</param>
    /// <returns>ServiceResult containing a ServiceStatus depending on the outcome</returns>
    public IServiceResult RemoveContact(string email) 
    {
        IServiceResult response = new ServiceResult();

        try
        {
            GetDataFromJSONFile();
            var contactToRemove = Contacts.FirstOrDefault(x => x.Email == email);

            if (contactToRemove == null)
            {
                response.Status = ServiceStatus.NOT_FOUND;
                return response;
            }
            else
            {
                Contacts.Remove(contactToRemove);
                //ContactsUpdated?.Invoke(this, new EventArgs());
                _fileService.SaveContentToFile(JsonConvert.SerializeObject(Contacts, jsonSettings));
                response.Status = ServiceStatus.SUCCESS;
                return response;
            }
        }
        catch
        {
            response.Status = ServiceStatus.FAILED;
            return response;
        }
    }

    /// <summary>
    /// Gets the list from the jsonfile and updates the contact with the provided email
    /// </summary>
    /// <param name="email">the provided email to update the correct contact</param>
    /// <param name="newFirstName">updated first name</param>
    /// <param name="newLastName">updated last name</param>
    /// <param name="newPhoneNumber">updated phone number</param>
    /// <param name="newEmail">updated email</param>
    /// <param name="newAddress">updated address</param>
    /// <param name="newPostalCode">updated postal code</param>
    /// <param name="newCity">updated city</param>
    /// <returns>ServiceResult containing a ServiceStatus depending on the outcome</returns>
    public IServiceResult UpdateContact(string email, string newFirstName, string newLastName, string newPhoneNumber, string newEmail, string newAddress, string newPostalCode, string newCity)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            GetDataFromJSONFile();
            var contactToUpdate = Contacts.FirstOrDefault(x => x.Email == email);

            if (contactToUpdate == null)
            {
                response.Status = ServiceStatus.NOT_FOUND;
                return response;
            }
            else
            {
                contactToUpdate.FirstName = newFirstName;
                contactToUpdate.LastName = newLastName;
                contactToUpdate.PhoneNumber = newPhoneNumber;
                contactToUpdate.Email = newEmail;
                contactToUpdate.Address = newAddress;
                contactToUpdate.PostalCode = newPostalCode;
                contactToUpdate.City = newCity;

                _fileService.SaveContentToFile(JsonConvert.SerializeObject(Contacts, jsonSettings));
                response.Status = ServiceStatus.SUCCESS;
                return response;
            }
        }
        catch
        {
            response.Status = ServiceStatus.FAILED;
            return response;
        }
    }

    /// <summary>
    /// Method used by most other methodes to get the data from the jsonfile and store it in the _contacts list
    /// </summary>
    /// <returns>The updated _contacts list</returns>
    public List<IContact> GetDataFromJSONFile()
    {
        var content = _fileService.GetContentFromFile();

        //Checks if the jsonfile is null or empty. If it is, an empty list is returned instead
        if (!string.IsNullOrEmpty(content))
        {
            Contacts = JsonConvert.DeserializeObject<List<IContact>>(content, jsonSettings)!;
        }
        return Contacts;
    }

    /// <summary>
    /// Simple validation to make sure no letter in the postal code and phone number
    /// </summary>
    /// <param name="number">Number input to be validated</param>
    /// <returns>True if all digits, else false</returns>
    public bool NumberValidation(string number)
    {
        if (string.IsNullOrEmpty(number))
        {
            return true;
        }
        else if (number.All(char.IsDigit))
        {
            return true;
        }

        return false;
    }
}
