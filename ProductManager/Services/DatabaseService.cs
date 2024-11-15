using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Services;

public class DatabaseService
{
    private SqliteConnection? sqliteConnection;
    private AppDbContext? appDbContext;

    public AppDbContext AppDbContext => appDbContext ?? throw new ArgumentNullException("Not initialize database yet!");

    public async Task Initialize(string dbPath)
    {
        sqliteConnection = new SqliteConnection($"Data Source={dbPath}");
        await sqliteConnection.OpenAsync();

        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(sqliteConnection)
            .Options;
        appDbContext = new AppDbContext(dbOptions);
        await appDbContext.Database.EnsureCreatedAsync();
    }
}
