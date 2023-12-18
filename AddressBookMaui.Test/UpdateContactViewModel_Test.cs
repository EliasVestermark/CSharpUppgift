using AddressBookMaui.ViewModels;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.Services;
using Moq;

namespace AddressBookMaui.Test;

public class UpdateContactViewModel_Test
{

    [Fact]
    public void RemoveContactFromList_OnSuccessShouldSetEntryEmailToEmptyString()
    {
        //Arrange
        string firstName = "Elias";
        string lastName = "Vestermark";
        string phoneNumber = "0701234567";
        string email = "elias@domain.com";
        string address = "Falmark";
        string postalCode = "123";
        string city = "Skellefteå";

        IContact contact = new ClassLibrary.Models.Contact(firstName, lastName, phoneNumber, email, address, postalCode, city);

        //Mocking to simulate filehandling
        var mockContactService = new Mock<IContactService>();

        UpdateContactViewModel viewModel = new UpdateContactViewModel(mockContactService.Object);

        //Act

        viewModel.RemoveContactFromList(contact);

        //Assert

        Assert.Equal(string.Empty, viewModel.EntryEmail);
    }
}
