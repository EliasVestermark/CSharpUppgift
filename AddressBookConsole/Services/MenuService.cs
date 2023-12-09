
using AddressBookConsole.Interfaces;
using ClassLibrary.Models;

namespace ClassLibrary.Services;

public class MenuService : IMenuService
{
    /// <summary>
    /// A field, type IContactService,to be used to reach the instance of ContactService created by DI
    /// </summary>
    private readonly IContactService _contactService;

    /// <summary>
    /// Constructor that takes a param then saves the value of the param to the field _contactService
    /// </summary>
    /// <param name="contactService">IContactService</param>
    public MenuService(IContactService contactService)
    {
        _contactService = contactService;
    }

    /// <summary>
    /// Shows the main menu. Uses a while loop to keep the program going until it's manually turned off
    /// Following menu methods uses a while loop for the same reason
    /// </summary>
    public void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            DisplayTitle("MAIN MENU");
            Console.WriteLine("1. Add contact");
            Console.WriteLine("2. Show all contacts");
            Console.WriteLine("x. Exit application");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowAddContactMenu();
                    break;

                case "2":
                    ShowAllContactsMenu();
                    break;

                case "x":
                    ShowExitMenu();
                    break;

                default:
                    Console.WriteLine("Invalid option, press any key then try again");
                    Console.ReadKey();
                    break;

            }
        }
    }

    /// <summary>
    /// Saves user input to a new contact which is then handled by _contactService.AddContact(contact)
    /// </summary>
    private void ShowAddContactMenu()
    {
        DisplayTitle("ADD CONTACT");

        Console.Write("First Name: ");
        string firstName = Console.ReadLine()!;

        Console.Write("Last Name: ");
        string lastName = Console.ReadLine()!;

        Console.Write("Phone Number: ");
        bool validPhoneNumber = int.TryParse(Console.ReadLine(), out int phoneNumber);

        //While loop to make sure input is only digits
        while (!validPhoneNumber)
        {
            Console.Write("Please enter a valid phone number: ");
            validPhoneNumber = int.TryParse(Console.ReadLine(), out phoneNumber);
        }

        Console.Write("Email: ");
        string email = Console.ReadLine()!;

        Console.Write("Address: ");
        string address = Console.ReadLine()!;

        Console.Write("Postal Code: ");
        bool validPostalCode = int.TryParse(Console.ReadLine(), out int postalCode);

        while (!validPostalCode)
        {
            Console.Write("Please enter a valid postal code: ");
            validPostalCode = int.TryParse(Console.ReadLine(), out postalCode);
        }

        Console.Write("City: ");
        string city = Console.ReadLine()!;

        Contact contact = new(firstName, lastName, phoneNumber, email, address, postalCode, city);
        var result = _contactService.AddContact(contact);

        switch (result.Status)
        {
            case Enums.ServiceStatus.SUCCESS:
                Console.WriteLine("The contact has been added successfully");
                Console.WriteLine("Press any button to continue");
                break;

            case Enums.ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine("A contact with the same email already exists");
                Console.WriteLine("Press any button to continue");
                break;

            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to add the contact, please try again");
                Console.WriteLine("Press any button to continue");
                break;
        }

        Console.ReadKey();
    }

    /// <summary>
    /// Menu to show the entire list of contacts
    /// </summary>
    private void ShowAllContactsMenu()
    {
        bool run = true;

        while (run)
        {
            DisplayTitle("SHOW ALL CONTACTS");

            //Gets the list every loop cycle incase it's been edited
            var result = _contactService.GetAllContacts();

            switch (result.Status)
            {
                case Enums.ServiceStatus.SUCCESS:

                    int index = 1;

                    if (result.Result is List<Contact> contactList)
                    {
                        if (!contactList.Any())
                        {
                            Console.WriteLine("No contacts has been added, the list is empty");
                            Console.WriteLine("Press any button to continue");
                            Console.ReadKey();
                            run = false;
                        }
                        else
                        {
                            Console.WriteLine($"View contact details or press (x) to go back to the main menu");
                            Console.WriteLine();

                            //Adds an index to each contact to be used to view detailed information
                            foreach (var contact in contactList)
                            {
                                Console.WriteLine($"{index}. Name: {contact.FirstName} {contact.LastName}");
                                index++;
                            }

                            var option = Console.ReadLine();

                            //Makes sure the input is either "x" or a valid index within the viewable list
                            if (option!.Equals("x", StringComparison.CurrentCultureIgnoreCase))
                            {
                                run = false;
                            }
                            else if (int.TryParse(option, out int optionInt) && optionInt <= index - 1 && optionInt >= 1)
                            {
                                ShowContactInformationMenu(optionInt);
                            }
                            else
                            {
                                Console.WriteLine("Invalid option, press any key then try again");
                                Console.ReadKey();
                            }
                        }
                    }
                    break;

                case Enums.ServiceStatus.NOT_FOUND:
                    Console.WriteLine("No contacts has been added, the list is empty");
                    Console.WriteLine("Press any button to continue");
                    Console.ReadKey();
                    run = false;
                    break;

                case Enums.ServiceStatus.FAILED:
                    Console.WriteLine("An error occured when trying to show the contact list, please try again");
                    Console.WriteLine("Press any button to continue");
                    Console.ReadKey();
                    break;
            }
        }

    }

    /// <summary>
    /// Shows detailed contact info of a specified contact depending on the input (param)
    /// </summary>
    /// <param name="option">The input from the user when they chose which contact to view information about</param>
    private void ShowContactInformationMenu(int option)
    {
        DisplayTitle("CONTACT INFORMATION");

        bool run = true;

        while (run)
        {
            var result = _contactService.GetContactInformation(option);

            switch (result.Status)
            {
                case Enums.ServiceStatus.SUCCESS:
                    if (result.Result is IContact contact)
                    {
                        DisplayTitle("CONTACT OPTIONS");

                        Console.WriteLine($"Press (r) to remove contact, (u) to update contact information or (x) to go back to the list");
                        Console.WriteLine();
                        Console.WriteLine($"Name: {contact.FirstName} {contact.LastName}");
                        Console.WriteLine($"Phone Number: {contact.PhoneNumber}");
                        Console.WriteLine($"Email: {contact.Email}");
                        Console.WriteLine($"Address: {contact.Address}");
                        Console.WriteLine($"       : {contact.PostalCode} {contact.City}");

                        var menuOption = Console.ReadLine();

                        switch (menuOption)
                        {
                            case "r":
                                ShowRemoveContactMenu($"{contact.FirstName} {contact.LastName}", contact.Email);

                                run = false;
                                break;

                            case "u":
                                ShowUpdateContactMenu($"{contact.FirstName} {contact.LastName}", contact.Email);
                                break;

                            case "x":
                                run = false;
                                break;

                            default:
                                Console.WriteLine("Invalid option, press any key then try again");
                                Console.ReadKey();
                                break;
                        }
                    }
                    break;

                case Enums.ServiceStatus.FAILED:
                    Console.WriteLine("An error occured when trying to show contact information.");
                    Console.WriteLine("Press any button to continue");
                    Console.ReadKey();
                    break;
            }
        }
    }

    /// <summary>
    /// Shows the menu for removing a contact
    /// </summary>
    /// <param name="name">Contact full name</param>
    /// <param name="email">Contact email</param>
    private void ShowRemoveContactMenu(string name, string email)
    {
        DisplayTitle("REMOVE CONTACT");
        Console.WriteLine($"Are you sure you want to remove \"{name}\" from the contact list? Confirm by typing the email ({email})");

        var option = Console.ReadLine();

        if (option == email)
        {
            var result = _contactService.RemoveContact(email);

            switch (result.Status)
            {
                case Enums.ServiceStatus.SUCCESS:
                    Console.WriteLine($"\"{name}\" has been removed from the contact list");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.ReadKey();
                    break;

                case Enums.ServiceStatus.FAILED:
                    Console.WriteLine("An error occured when trying to remove the contact, please try again");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.ReadKey();
                    break;

                case Enums.ServiceStatus.NOT_FOUND:
                    Console.WriteLine($"Contact \"{name}\", email: {email}, does not seem to exist");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.ReadKey();
                    break;
            }
        }
        else
        {
            Console.WriteLine("Your input doesn't match the email so the contact has not been deleted");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Shows the menu for updating a contact.
    /// </summary>
    /// <param name="name">Used to display the original name of the contact</param>
    /// <param name="email">used to fetch the correct contact to update</param>
    private void ShowUpdateContactMenu(string name, string email)
    {
        DisplayTitle("UPDATE CONTACT INFORMATION");

        Console.Write("First Name: ");
        string newFirstName = Console.ReadLine()!;

        Console.Write("Last Name: ");
        string newLastName = Console.ReadLine()!;

        Console.Write("Phone Number: ");
        bool validPhoneNumber = int.TryParse(Console.ReadLine(), out int newPhoneNumber);

        while (!validPhoneNumber)
        {
            Console.Write("Please enter a valid phone number: ");
            validPhoneNumber = int.TryParse(Console.ReadLine(), out newPhoneNumber);
        }

        Console.Write("Email: ");
        string newEmail = Console.ReadLine()!;

        Console.Write("Address: ");
        string newAddress = Console.ReadLine()!;

        Console.Write("Postal Code: ");
        bool validPostalCode = int.TryParse(Console.ReadLine(), out int newPostalCode);

        while (!validPostalCode)
        {
            Console.Write("Please enter a valid postal code: ");
            validPostalCode = int.TryParse(Console.ReadLine(), out newPostalCode);
        }

        Console.Write("City: ");
        string newCity = Console.ReadLine()!;

        var result = _contactService.UpdateContact(email, newFirstName, newLastName, newPhoneNumber, newEmail, newAddress, newPostalCode, newCity);

        switch (result.Status)
        {
            case Enums.ServiceStatus.SUCCESS:
                Console.WriteLine("The contact information has been updates successfully");
                Console.WriteLine("Press any key to go back to the list");
                Console.ReadKey();
                break;

            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to update the contact, please try again");
                Console.WriteLine("Press any key to go back to the list");
                Console.ReadKey();

                break;

            case Enums.ServiceStatus.NOT_FOUND:
                Console.WriteLine($"Contact \"{name}\", email: {email}, does not seem to exist");
                Console.WriteLine("Press any key to go back to the list");
                Console.ReadKey();
                break;
        }
    }

    /// <summary>
    /// Exit menu
    /// </summary>
    private void ShowExitMenu()
    {
        bool run = true;

        while (run)
        {
            DisplayTitle("EXIT APPLICATION");

            Console.WriteLine("Are you sure you want to exit? (y/n)");
            var option = Console.ReadLine();

            //Switch för att hantera olika inputs
            switch (option)
            {
                case "y":
                    Environment.Exit(0);
                    break;

                case "n":
                    run = false;
                    break;

                default:
                    Console.WriteLine("Invalid option, press any key then try again");
                    Console.ReadKey();
                    break;
            }
        }
    }

    /// <summary>
    /// Method used to display the title of each menu
    /// </summary>
    /// <param name="title">Title name</param>
    private void DisplayTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"## {title} ##");
        Console.WriteLine();
    }
}
