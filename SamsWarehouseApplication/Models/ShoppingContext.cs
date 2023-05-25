using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace SamsWarehouseApplication.Models
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext(DbContextOptions options) : base(options) 
        {

        }

        public DbSet<Product> Products { get; set;}
        public DbSet<AppUser> AppUsers { get; set;} 
        public DbSet<ShoppingListItem> ShoppingItems { get; set;}
        public DbSet<ShoppingList> ShoppingLists { get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ShoppingListItem>()
                .HasOne(x => x.ShoppingList)
                .WithMany(x => x.ShoppingListItems)
                .HasForeignKey(x => x.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ShoppingList>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserShoppingList)
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ShoppingListItem>()
                .HasOne(x => x.Product)
                .WithMany(x => x.ProductList)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
