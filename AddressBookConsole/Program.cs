using AddressBookConsole.Interfaces;
using AddressBookConsole.Services;
using ClassLibrary.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddSingleton<IFileService>(new FileService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Adressbok")));
    services.AddSingleton<IContactService, ContactService>();
    services.AddSingleton<IMenuService, MenuService>();
}).Build();

builder.Start();
Console.Clear();

var menuService = builder.Services.GetRequiredService<IMenuService>();

menuService.ShowMainMenu();
