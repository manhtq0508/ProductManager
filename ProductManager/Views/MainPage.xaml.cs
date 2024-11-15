using ProductManager.ViewModels;
using ProductManager.Views;

namespace ProductManager.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Yield();

        if (BindingContext is MainViewModel viewModel)
        {
            await viewModel.InitializeDbAndNavigate();
        }
    }
}
