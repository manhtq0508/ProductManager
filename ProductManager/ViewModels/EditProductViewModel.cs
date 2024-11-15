using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.Entities;
using ProductManager.Interfaces;

namespace ProductManager.ViewModels;

[QueryProperty("SelectedProduct", "SelectedProduct")]
public partial class EditProductViewModel(IProductRepo productRepo) : ObservableObject
{
    [ObservableProperty]
    private Product? selectedProduct;

    [RelayCommand]
    private async Task SaveChange()
    {
        if (SelectedProduct == null)
            return;

        await productRepo.UpdateProductAsync(SelectedProduct);
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
