using ProductManager.ViewModels;

namespace ProductManager.Views;

public partial class AddProductPage : ContentPage
{
	public AddProductPage(AddProductViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}