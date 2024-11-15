using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.Entities;
using ProductManager.Interfaces;
using ProductManager.Views;
using System.Collections.ObjectModel;

namespace ProductManager.ViewModels;

public partial class ProductViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<Product> products = new ObservableCollection<Product>();

    private List<Product> selectedProducts = new List<Product>();

    [ObservableProperty]
    SelectionMode currentSelectionMode = SelectionMode.Single;

    private readonly IProductRepo productRepo;

    public ProductViewModel(IProductRepo productRepo)
    {
        this.productRepo = productRepo;
    }

    public async Task LoadProductDataAsync()
    {
        foreach (var product in await productRepo.GetProductsAsync())
        {
            Products.Add(product);
        }
    }

    public void UpdateSelectedItems(List<Product> selectedProducts)
    {
        this.selectedProducts = selectedProducts;
    }

    [RelayCommand]
    private async Task AddProduct()
    {
        await Shell.Current.GoToAsync(nameof(AddProductPage));
    }

    [RelayCommand]
    private async Task EditProduct()
    {
        if (CurrentSelectionMode == SelectionMode.Multiple)
        {
            if (App.Current?.MainPage != null)
                await App.Current.MainPage.DisplayAlert("Lỗi", "Chỉ có thể chọn 1 sản phẩm để sửa", "OK");
            return;
        }

        if (selectedProducts == null || selectedProducts.Count == 0)
        {
            if (App.Current?.MainPage != null)
                await App.Current.MainPage.DisplayAlert("Lỗi", "Vui lòng chọn sản phẩm cần sửa", "OK");
            return;
        }


        var param = new Dictionary<string, object>
        {
            { "SelectedProduct", selectedProducts.First()  }
        };
        await Shell.Current.GoToAsync(nameof(EditProductPage), param);
    }

    [RelayCommand]
    private void DeleteProduct()
    {
        // Delete a product
    }
}
