using AddressBookConsole.Interfaces;
using ClassLibrary.Interfaces;
using ClassLibrary.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//Dependency injection to create and handle instances of different classes
var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddSingleton<IFileService>(new FileService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Adressbok")));
    services.AddSingleton<IContactService, ContactService>();
    services.AddSingleton<IMenuService, MenuService>();
}).Build();

builder.Start();
Console.Clear();

//variabel that contains the instance of IMenuService created by DI
var menuService = builder.Services.GetRequiredService<IMenuService>();

var contactService = builder.Services.GetRequiredService<IContactService>();
//Gets the data from the jsonfile and saves it to the contact list on program startup
contactService.GetDataFromJSONFile();

menuService.ShowMainMenu();
