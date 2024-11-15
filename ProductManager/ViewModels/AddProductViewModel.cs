using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.Entities;
using ProductManager.Interfaces;

namespace ProductManager.ViewModels;

public partial class AddProductViewModel(IProductRepo productRepo) : ObservableObject
{
    [ObservableProperty]
    private string id = "";

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private int price = 0;

    [ObservableProperty]
    private int amount = 0;

    [RelayCommand]
    private async Task AddProduct()
    { 
        if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Name))
        {
            if (App.Current?.MainPage != null)
                await App.Current.MainPage.DisplayAlert("Lỗi", "Mã sản phẩm và tên không được để trống", "OK");
            return;
        }

        Product product = new Product
        {
            Id = Id,
            Name = Name,
            Price = Price,
            Amount = Amount
        };
        
        try
        {
            await productRepo.AddProductAsync(product);
            await Shell.Current.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            if (App.Current?.MainPage != null)
                await App.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
            return;
        }
    }

    [RelayCommand]
    private async Task Cancel() 
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
