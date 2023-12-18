
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.Enums;
using ClassLibrary.Models.Responses;
using ClassLibrary.Services;
using Moq;
using AddressBookMaui.ViewModels;

namespace AddresBookMaui.Test;

public class UpdateContactViewModel_Test
{
    /// <summary>
    /// Test to verify the statusmessage if email doesnt match when RemoveContactFromList is being run
    /// </summary>
    [Fact]
    public void RemoveContactFromList_ShouldNotRemoveContactFromListWhenEmailDoesntMatch_ThenSetStatusMessage()
    {
        // Arrange
        string firstName = "Elias";
        string lastName = "Vestermark";
        string phoneNumber = "0701234567";
        string email = "elias@domain.com";
        string address = "Falmark";
        string postalCode = "123";
        string city = "Skellefteå";

        IContact contact = new ClassLibrary.Models.Contact(firstName, lastName, phoneNumber, email, address, postalCode, city);

        var mockContactService = new Mock<IContactService>();
        UpdateContactViewModel viewModel = new UpdateContactViewModel(mockContactService.Object);

        // Act
        viewModel.RemoveContactFromList(contact);

        // Assert
        Assert.Equal("The email doesn't match, try again", viewModel.StatusMessage);
    }
}
