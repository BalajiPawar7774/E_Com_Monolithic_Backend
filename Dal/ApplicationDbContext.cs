using E_Com_Monolithic.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Com_Monolithic.Dal
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductImage> productImages { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<Cart> carts { get; set; }
    }
}
