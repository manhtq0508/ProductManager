using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.Entities;
using ProductManager.Interfaces;
using ProductManager.Services;
using ProductManager.Views;

namespace ProductManager.ViewModels;

public partial class MainViewModel(DatabaseService dbService, IProductRepo productRepo) : ObservableObject
{
    public async Task InitializeDbAndNavigate()
    {
        // !IMPORTANT: REMOVE THIS LINE WHEN COMPLETED
        // Preferences.Clear();

        if (!string.IsNullOrEmpty(Preferences.Get("DB_PATH", "")))
        {
            ShowLoadingScreen = true;

            var dbPath = Preferences.Get("DB_PATH", "");
            await TryInitializeDatabaseAsync(dbPath);
            await Shell.Current.GoToAsync($"//{nameof(ProductPage)}");
        }
    }
    private async Task TryInitializeDatabaseAsync(string dbPath)
    {
        try
        {
            // !IMPORTANT: Do not call Initialize method directly
            // Initialize database in background thread
            // to prevent UI blocking
            await Task.Run(async () => await dbService.Initialize(dbPath));
        }
        catch (Exception ex)
        {
            if (App.Current?.MainPage != null)
                await App.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
        }
    }

    private async Task AddDefaultDataAsync()
    {
        var products = new List<Product>
        {
            new Product { Id = "SP1", Name = "Product 1", Price = 1000, Amount = 5 },
            new Product { Id = "SP2", Name = "Product 2", Price = 95000, Amount = 4 },
            new Product { Id = "SP3", Name = "Product 3", Price = 12000, Amount = 2 },
            new Product { Id = "SP4", Name = "Product 4", Price = 42100, Amount = 1 },
            new Product { Id = "SP5", Name = "Product 5", Price = 54200, Amount = 1 },
            new Product { Id = "SP6", Name = "Product 6", Price = 112000, Amount = 3 },
        };

        await productRepo.AddProductAsync(products);
    }

    [ObservableProperty]
    private string dataFilePath = "";

    [ObservableProperty]
    private bool showLoadingScreen = false;

    [RelayCommand]
    private async Task SelectDataFile()
    {
        FilePickerFileType fileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
            {DevicePlatform.WinUI, new string[] {"db", "sqlite3", "sqlite", ".xlsx"} }
        });

        var file = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Chọn file dữ liệu",
            FileTypes = fileType
        });

        if (file == null) return;

        DataFilePath = file.FullPath;
    }

    [RelayCommand]
    private async Task ImportData()
    {
        if (string.IsNullOrWhiteSpace(DataFilePath)) 
        {
            if (App.Current?.MainPage != null)
                await App.Current.MainPage.DisplayAlert("Lỗi", "Vui lòng chọn file!", "OK");

            return;
        }

        if (Path.GetExtension(DataFilePath) == ".xlsx")
        {
            await ImportFromExcel();
            return;
        }

        await TryInitializeDatabaseAsync(DataFilePath);

        Preferences.Set("DB_PATH", DataFilePath);
        await Shell.Current.GoToAsync($"//{nameof(ProductPage)}");
    }

    private async Task ImportFromExcel()
    {
        if (App.Current?.MainPage != null)
            await App.Current.MainPage.DisplayAlert("Lỗi", "hiện chưa hỗ trợ!", "OK");
    }

    [RelayCommand]
    private async Task UsingDefaultData()
    {
        ShowLoadingScreen = true;

        var filePath = Path.Combine(FileSystem.AppDataDirectory, "default.db");

        if (File.Exists(filePath))
            File.Delete(filePath);

        await TryInitializeDatabaseAsync(filePath);
        await AddDefaultDataAsync();

        Preferences.Set("DB_PATH", filePath);
        await Shell.Current.GoToAsync($"//{nameof(ProductPage)}");
    }

    [RelayCommand]
    private async Task ContinueWithoutData()
    {
        ShowLoadingScreen = true;

        var fileName = $"empty_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.db";
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        await TryInitializeDatabaseAsync(filePath);

        Preferences.Set("DataFilePath", filePath);
        await Shell.Current.GoToAsync($"//{nameof(ProductPage)}");
    }
}
