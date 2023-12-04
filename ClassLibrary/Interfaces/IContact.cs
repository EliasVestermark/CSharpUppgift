namespace ClassLibrary.Models
{
    public interface IContact
    {
        string Address { get; set; }
        string City { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int PhoneNumber { get; set; }
        int PostalCode { get; set; }
    }
}