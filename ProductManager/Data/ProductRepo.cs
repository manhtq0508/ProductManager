using Microsoft.EntityFrameworkCore;
using ProductManager.Entities;
using ProductManager.Interfaces;
using ProductManager.Services;

namespace ProductManager.Data;

public class ProductRepo(DatabaseService dbService) : IProductRepo
{
    public async Task AddProductAsync(Product product)
    {
        bool isExsist = await dbService.AppDbContext.Products.AnyAsync(p => p.Id == product.Id);

        if (isExsist)
            throw new Exception("Mã sản phẩm đã tồn tại");

        await dbService.AppDbContext.Products.AddAsync(product);
        await dbService.AppDbContext.SaveChangesAsync();
    }

    public async Task AddProductAsync(List<Product> products)
    {
        foreach (var product in products)
        {
            bool isExist = await dbService.AppDbContext.Products.AnyAsync(p => p.Id == product.Id);

            if (isExist)
                throw new Exception("Mã sản phẩm đã tồn tại");
        }

        await dbService.AppDbContext.Products.AddRangeAsync(products);
        await dbService.AppDbContext.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Product product)
    {
        dbService.AppDbContext.Products.Remove(product);
        await dbService.AppDbContext.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(List<Product> products)
    {
        dbService.AppDbContext.Products.RemoveRange(products);
        await dbService.AppDbContext.SaveChangesAsync();
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await dbService.AppDbContext.Products.ToListAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        dbService.AppDbContext.Products.Update(product);
        await dbService.AppDbContext.SaveChangesAsync();
    }
}
