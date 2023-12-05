
using AddressBookConsole.Interfaces;
using ClassLibrary.Models;

namespace ClassLibrary.Services;

public class MenuService : IMenuService
{
    private readonly IContactService _contactService;

    public MenuService(IContactService contactService)
    {
        _contactService = contactService;
    }

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

    private void ShowAddContactMenu()
    {
        DisplayTitle("ADD CONTACT");

        Console.Write("First Name: ");
        string firstName = Console.ReadLine()!;

        Console.Write("Last Name: ");
        string lastName = Console.ReadLine()!;

        Console.Write("Phone Number: ");
        bool validPhoneNumber = int.TryParse(Console.ReadLine(), out int phoneNumber);
        
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

        IContact contact = new Contact(firstName, lastName, phoneNumber, email, address, postalCode, city);

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

    private void ShowAllContactsMenu()
    {
        var result = _contactService.GetAllContacts();

        switch (result.Status)
        {
            case Enums.ServiceStatus.SUCCESS:

                int index = 1;
                
                if (result.Result is IEnumerable<IContact> contactList)
                {
                    bool run = true;

                    while (run)
                    {
                        DisplayTitle("SHOW ALL CONTACTS");

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

                            foreach (var contact in contactList)
                            {
                                Console.WriteLine($"{index}. Name: {contact.FirstName} {contact.LastName}");
                                index++;
                            }

                            var option = Console.ReadLine();

                            if (option!.Equals("x", StringComparison.CurrentCultureIgnoreCase))
                            {
                                run = false;
                            }
                            else if (int.TryParse(option, out int optionInt) && optionInt <= index && optionInt >= 1)
                            {
                                ShowContactInformationMenu(optionInt);
                                index = 1;
                            }
                            else
                            {
                                Console.WriteLine("Invalid option, press any key then try again");
                                Console.ReadKey();
                            }
                        }
                    }

                }
                break;

            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to add the contact, please try again");
                Console.WriteLine("Press any button to continue");
                Console.ReadKey();
                break;
        }
    }

    private void ShowContactInformationMenu(int option)
    {
        DisplayTitle("CONTACT INFORMATION");

        var result = _contactService.GetContactInformation(option);

        switch (result.Status)
        {
            case Enums.ServiceStatus.SUCCESS:
                if (result.Result is IContact contact)
                {
                    bool run = true;

                    while (run)
                    {
                        DisplayTitle("EMPLOYEE OPTIONS");

                        Console.WriteLine($"Press (r) to remove employee, (u) to update employee information or (x) to go back to the list");
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
                }
                break;

            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to add the contact, please try again");
                Console.WriteLine("Press any button to continue");
                break;
        }
    }

    private void ShowRemoveContactMenu(string name, string email)
    {
        bool run = true;

        while (run)
        {
            DisplayTitle("REMOVE EMPLOYEE");
            Console.WriteLine($"Are you sure you want to remove \"{name}\" from the employee list? (y/n)");

            var option = Console.ReadLine();

            switch (option)
            {
                case "y":
                    var result = employeeService.RemoveEmployee(id);

                    switch (result.Status)
                    {
                        case Enums.Status.SUCCESS:
                            Console.WriteLine($"\"{name}\" has been removed from the employee list");
                            Console.WriteLine("Press any key to go back to the list");
                            Console.ReadKey();
                            break;

                        case Enums.Status.FAILED:
                            Console.WriteLine("ERROR, oopsie");
                            Console.WriteLine("Press any key to go back to the list");
                            Console.ReadKey();
                            break;

                        case Enums.Status.NOT_FOUND:
                            Console.WriteLine($"Employee \"{name}\", id: {id}, does not seem to exist");
                            Console.WriteLine("Press any key to go back to the list");
                            Console.ReadKey();
                            break;
                    }

                    run = false;
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

    private void ShowUpdateContactMenu(string name, string email)
    {
        DisplayTitle("UPDATE EMPLOYEE INFORMATION");

        Console.Write("New Full Name: ");
        var newName = Console.ReadLine()!;

        Console.Write("New Position: ");
        var newPosition = Console.ReadLine()!;

        var result = employeeService.UpdateEmployee(id, newName, newPosition);

        switch (result.Status)
        {
            case Enums.Status.SUCCESS:
                Console.WriteLine("The employee information has been updates successfully");
                Console.WriteLine("Press any key to go back to the list");
                Console.ReadKey();
                break;

            case Enums.Status.FAILED:
                Console.WriteLine("ERROR, oopsie");
                Console.WriteLine("Press any key to go back to the list");
                Console.ReadKey();
                break;

            case Enums.Status.NOT_FOUND:
                Console.WriteLine($"Employee \"{oldName}\", id: {id}, does not seem to exist");
                Console.WriteLine("Press any key to go back to the list");
                Console.ReadKey();
                break;
        }
    }

    private void ShowExitMenu()
    {
        bool run = true;

        while (run)
        {
            DisplayTitle("EXIT APPLICATION");

            Console.WriteLine("Are you sure you want to exit? (y/n)");
            var option = Console.ReadLine();

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

    private void DisplayTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"## {title} ##");
        Console.WriteLine();
    }
}
