using Microsoft.EntityFrameworkCore;
using ProductManager.Entities;

namespace ProductManager.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
