using Microsoft.EntityFrameworkCore;
using Store.ApplicationCore.Entities;

namespace Store.Infrastructure.Persistence.Contexts
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<Customer>()
            .HasIndex(p => new { p.Firstname, p.Lastname, p.DateOfBirth })
            .IsUnique(true);
        }
    }
}