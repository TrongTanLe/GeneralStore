using generalStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using generalStore.Models.ViewModels;

namespace generalStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TransactStatus> TransactStatus { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<generalStore.Models.ViewModels.RegisterViewModel>? RegisterViewModel { get; set; }
        public DbSet<generalStore.Models.ViewModels.CartItem>? CartItem { get; set; }
        public DbSet<generalStore.Models.ViewModels.MuaHangVM>? MuaHangVM { get; set; }
    }
}