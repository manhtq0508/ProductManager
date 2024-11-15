using ProductManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Interfaces;

public interface IProductRepo
{
    Task<List<Product>> GetProductsAsync();
    Task AddProductAsync(Product product);
    Task AddProductAsync(List<Product> products);
    Task DeleteProductAsync(Product product);
    Task DeleteProductAsync(List<Product> products);
    Task UpdateProductAsync(Product product);
}
