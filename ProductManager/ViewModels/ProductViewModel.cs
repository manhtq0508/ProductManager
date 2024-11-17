using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductManager.Entities;
using ProductManager.Interfaces;
using ProductManager.Utils;
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
    [ObservableProperty]
    private string deleteStatus = "Chế độ xoá";

    private readonly IProductRepo productRepo;

    public ProductViewModel(IProductRepo productRepo)
    {
        this.productRepo = productRepo;
    }

    public async Task LoadProductDataAsync()
    {
        Products.Clear();
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
    private async Task DeleteProduct()
    {
        if (DeleteStatus == "Chế độ xoá")
        {
            DeleteStatus = "Xoá / Thoát xoá";
            CurrentSelectionMode = SelectionMode.Multiple;
            return;
        }

        DeleteStatus = "Chế độ xoá";

        if (selectedProducts == null || selectedProducts.Count == 0) return;

        if (App.Current?.MainPage != null)
        {
            bool result = await App.Current.MainPage.DisplayAlert("Xác nhận", "Bạn có chắc chắn muốn xoá sản phẩm đã chọn?", "Có", "Không");
            if (!result) return;
        }

        await productRepo.DeleteProductAsync(selectedProducts);
        
        foreach (var product in selectedProducts)
        {
            Products.Remove(product);
        }

        selectedProducts.Clear();
        CurrentSelectionMode = SelectionMode.Single;
    }

    [RelayCommand]
    private async Task ImportData()
     {
        FilePickerFileType fileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
            {DevicePlatform.WinUI, new string[] {".xlsx"} }
        });

        var file = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Chọn file dữ liệu",
            FileTypes = fileType
        });

        if (file == null) return;
        
        await Toast.Make("Đang import dữ liệu", CommunityToolkit.Maui.Core.ToastDuration.Short, 12).Show();

        List<Product> newProducts = new List<Product>();

        try
        {
            newProducts = await Task.Run(() => ExcelHelper.Import(file.FullPath));
            await productRepo.AddProductAsync(newProducts);
        }
        catch (Exception ex)
        {
            if (App.Current?.MainPage != null)
            {
                App.Current?.MainPage.DisplayAlert("Lỗi", ex.Message, "Ok");
                return;
            }
        }

        foreach (var product in newProducts) { Products.Add(product); }

        await Toast.Make("Import dữ liệu hoàn tất", CommunityToolkit.Maui.Core.ToastDuration.Short, 12).Show();
    }

    [RelayCommand]
    private async Task ExportData()
    {
        if (App.Current?.MainPage != null)
        {
            bool isExcel = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn muốn xuất file nào?", "Excel", "Pdf");
            if (isExcel)
            {
                await Task.Run(() => ExportExcel());
            }
            else
            {
                await Task.Run(() => ExportPdf());
            }
        }
    }

    private async void ExportExcel()
    {
        if (Products ==  null || Products.Count == 0) { return; }

        var file = await GetSaveFileName("xlsx");

        await Toast.Make($"Đang xuất file\n{file}", CommunityToolkit.Maui.Core.ToastDuration.Short, 12).Show();
        await Task.Run(() => ExcelHelper.Export(file, Products.ToList<Product>()));
        await Toast.Make($"Đã xuất file thành công\n{file}", CommunityToolkit.Maui.Core.ToastDuration.Short, 12).Show();
    }

    private async void ExportPdf()
    {
        if (Products == null || Products.Count == 0) { return; }

        var file = await GetSaveFileName("pdf");

        await Toast.Make($"Đang xuất file\n{file}", CommunityToolkit.Maui.Core.ToastDuration.Short, 12).Show();
        await Task.Run(() => PdfHelper.Export(file, Products.ToList<Product>()));
        await Toast.Make($"Đã xuất file thành công\n{file}", CommunityToolkit.Maui.Core.ToastDuration.Short, 12).Show();
    }

    private async Task<string> GetSaveFileName(string fileExt)
    {
        var path = "";
        var folder = await FolderPicker.Default.PickAsync();

        if (folder.IsSuccessful && folder.Folder != null)
            path = folder.Folder.Path;

        return Path.Combine(path, $"ProductManager_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.{fileExt}");
    }
}