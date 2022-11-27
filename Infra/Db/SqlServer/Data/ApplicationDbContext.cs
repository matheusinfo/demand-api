using IWantApp.Domain.Product;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Infra.Db.SqlServer.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>()
            .Property(product => product.Name).IsRequired();
        builder.Entity<Product>()
            .Property(product => product.Description).HasMaxLength(255);

        builder.Entity<Category>()
            .Property(category => category.Name).IsRequired();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        configuration.Properties<string>()
            .HaveMaxLength(100);
    }
}
