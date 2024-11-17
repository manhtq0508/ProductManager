using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ProductManager.Data;
using ProductManager.Interfaces;
using ProductManager.Services;
using ProductManager.ViewModels;
using ProductManager.Views;

namespace ProductManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<ProductPage>();
            builder.Services.AddSingleton<ProductViewModel>();

            builder.Services.AddSingleton<AddProductPage>();
            builder.Services.AddSingleton<AddProductViewModel>();

            builder.Services.AddSingleton<EditProductPage>();
            builder.Services.AddSingleton<EditProductViewModel>();

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<IProductRepo, ProductRepo>();

            return builder.Build();
        }
    }
}
