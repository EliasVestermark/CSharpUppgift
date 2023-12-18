using AddressBookMaui.ViewModels;
using AddressBookMaui.Views;
using ClassLibrary.Interfaces;
using ClassLibrary.Services;
using Microsoft.Extensions.Logging;

namespace AddressBookMaui
{
    public static class MauiProgram
    {
        //Dependency injection to create and handle instances of different classes
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<ContactListPage>();
            builder.Services.AddTransient<ContactListViewModel>();
            builder.Services.AddSingleton<UpdateContactPage>();
            builder.Services.AddSingleton<UpdateContactViewModel>();
            builder.Services.AddSingleton<AddContactPage>();
            builder.Services.AddSingleton<AddContactViewModel>();
            builder.Services.AddSingleton<IFileService>(new FileService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Adressbok")));
            builder.Services.AddSingleton<IContactService, ContactService>();

            builder.Logging.AddDebug();
            return builder.Build();
        }
    }
}
