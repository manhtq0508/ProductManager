using ProductManager.Entities;
using ProductManager.ViewModels;

namespace ProductManager.Views;

public partial class ProductPage : ContentPage
{
    private ProductViewModel productViewModel;
	public ProductPage(ProductViewModel viewModel)
	{
		InitializeComponent();
        productViewModel = viewModel;
        BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await productViewModel.LoadProductDataAsync();
    }

    private void ProductCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is ProductViewModel viewModel)
        {
            List<Product> selectedProducts = e.CurrentSelection.Cast<Product>().ToList();

            viewModel.UpdateSelectedItems(selectedProducts);
        }
    }
}