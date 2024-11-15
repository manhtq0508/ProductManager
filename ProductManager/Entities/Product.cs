using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ProductManager.Entities;

[PrimaryKey(nameof(Id))]
public partial class Product : ObservableObject
{
    [ObservableProperty]
    private string id = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private int price;

    [ObservableProperty]
    private int amount;

    public Product()
    {
    }

    public Product(string id, string name, int price, int amount)
    {
        Id = id;
        Name = name;
        Price = price;
        Amount = amount;
    }
}