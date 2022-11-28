using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Microsoft.eShopWeb.Infrastructure.Identity;

using EntityFrameworkCore.Design;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    
    // public class AppIdentityDbContextDesignFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
    // {
    //     public AppIdentityDbContext CreateDbContext(string[] args)
    //     {
    //         var optionsBuilder = new DbContextOptionsBuilder()
    //             .UseSqlServer("Server=tcp:learn-last-task-sqldb.database.windows.net,1433;Initial Catalog=public-api-db-identity;Persist Security Info=False;User ID=admin123;Password=3884FBFE-4FCC-48BF-A52E-D0C679CB8735;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    //         return new AppIdentityDbContext(optionsBuilder.Options);
    //     }
    // }
}
