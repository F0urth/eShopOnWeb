using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.Infrastructure.Data;

using EntityFrameworkCore.Design;
using Identity;

public class CatalogContext : DbContext
{
    #pragma warning disable CS8618 // Required by Entity Framework
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) {}

    public DbSet<Basket> Baskets { get; set; }
    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<CatalogBrand> CatalogBrands { get; set; }
    public DbSet<CatalogType> CatalogTypes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    // public class AppIdentityDbContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    // {
    //     public CatalogContext CreateDbContext(string[] args)
    //     {
    //         var optionsBuilder = new DbContextOptionsBuilder()
    //             .UseSqlServer("Server=tcp:learn-last-task-sqldb.database.windows.net,1433;Initial Catalog=public-api-db-data;Persist Security Info=False;User ID=admin123;Password=3884FBFE-4FCC-48BF-A52E-D0C679CB8735;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    //         return new CatalogContext(optionsBuilder.Options);
    //     }
    // }
}
