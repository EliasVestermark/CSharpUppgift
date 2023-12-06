
namespace ClassLibrary.Models;

public class Contact(string firstName, string lastName, int phoneNumber, string email, string address, int postalCode, string city) : IContact
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public int PhoneNumber { get; set; } = phoneNumber;
    public string Email { get; set; } = email;
    public string Address { get; set; } = address;
    public int PostalCode { get; set; } = postalCode;
    public string City { get; set; } = city;
}
