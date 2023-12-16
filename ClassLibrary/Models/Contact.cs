
namespace ClassLibrary.Models;

/// <summary>
/// Class for Contact-object
/// The first constructor is used in the console app where the values are provided before creating the new Contact.
/// The Empty constructor is used in the maui app where the values are provided after the new Contact is created.
/// </summary>

public class Contact : IContact
{
    public Contact(string firstName, string lastName, string phoneNumber, string email, string address, string postalCode, string city)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        City = city;
        PostalCode = postalCode;
    }

    public Contact()
    {
        
    }

    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";
    public string Index { get; set; } = null!;
}
