
namespace ClassLibrary.Models;

public class Contact : IContact
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int PostalCode { get; set; }
    public string City { get; set; } = null!;
}
