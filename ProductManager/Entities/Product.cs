using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace ProductManager.Entities;

[PrimaryKey(nameof(Id))]
public partial class Product : ObservableObject
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required int Price { get; set; }
    public required int Amount { get; set; }
}
