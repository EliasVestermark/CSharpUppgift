
using AddressBookConsole.Interfaces;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Models.Responses;
using ClassLibrary.Services;
using Moq;
using System;

namespace AddressBookConsole.Test;

public class ContactService_Test
{
    /// <summary>
    /// Test to see if AddContact works, specifically when return status is SUCCESS
    /// </summary>

    [Fact]
    public void AddContact_ShouldSuccessfullyAddOneContactToList_ThenReturnServiceResultStatusSuccess()
    {
        // Arrange
        string firstName = "Elias";
        string lastName = "Vestermark";
        int phoneNumber = 0701234567;
        string email = "elias@domain.com";
        string address = "Falmark";
        int postalCode = 123;
        string city = "Skellefteå";

        Contact contact = new Contact(firstName, lastName, phoneNumber, email, address, postalCode, city);

        //Mocking to simulate filehandling
        var mockFileService = new Mock<IFileService>();
        IContactService contactService = new ContactService(mockFileService.Object);

        IServiceResult testContent = new ServiceResult();
        testContent.Status = ServiceStatus.SUCCESS;

        // Act
        var response = contactService.AddContact(contact);

        // Assert
        Assert.Equal(testContent.Status, response.Status);
    }
}
