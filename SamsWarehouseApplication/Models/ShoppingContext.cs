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
            string salt = BCrypt.Net.BCrypt.GenerateSalt(10);
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

            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    AppUserId = 1,
                    UserEmail = "test@gmail.com",
                    UserPasswordHash = BCrypt.Net.BCrypt.HashPassword("password", salt)
                }
                );

            builder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    ProductName = "Granny Smith Apples",
                    ProductUnit = "1kg",
                    ProductPrice = 5.50,
                },

                new Product
                {
                    ProductId = 2,
                    ProductName = "Fresh Tomatoes",
                    ProductUnit = "500g",
                    ProductPrice = 5.90,
                },

                new Product
                {
                    ProductId = 3,
                    ProductName = "Watermelon",
                    ProductUnit = "Whole",
                    ProductPrice = 6.60,
                },

                new Product
                {
                    ProductId = 4,
                    ProductName = "Cucumber",
                    ProductUnit = "1 whole",
                    ProductPrice = 1.90,
                },

                new Product
                {
                    ProductId = 5,
                    ProductName = "Red Potato Washed",
                    ProductUnit = "1kg",
                    ProductPrice = 4.00,
                },

                new Product
                {
                    ProductId = 6,
                    ProductName = "Red Tipped Bananas",
                    ProductUnit = "1kg",
                    ProductPrice = 4.90,
                },

                new Product
                {
                    ProductId = 7,
                    ProductName = "Red Onion",
                    ProductUnit = "1kg",
                    ProductPrice = 3.50,
                },

                new Product
                {
                    ProductId = 8,
                    ProductName = "Carrots",
                    ProductUnit = "1kg",
                    ProductPrice = 2.00,
                },

                new Product
                {
                    ProductId = 9,
                    ProductName = "Iceburg Lettuce",
                    ProductUnit = "1",
                    ProductPrice = 2.50,
                },

                new Product
                {
                    ProductId = 10,
                    ProductName = "Helga's Wholemeal",
                    ProductUnit = "1",
                    ProductPrice = 3.70,
                },

                new Product
                {
                    ProductId = 11,
                    ProductName = "Free Range Chicken",
                    ProductUnit = "1kg",
                    ProductPrice = 7.50,
                },

                new Product
                {
                    ProductId = 12,
                    ProductName = "Manning Valley 6-pk",
                    ProductUnit = "6 eggs",
                    ProductPrice = 3.60,
                },

                new Product
                {
                    ProductId = 13,
                    ProductName = "A2 Light Milk",
                    ProductUnit = "1 litre",
                    ProductPrice = 2.90,
                },

                new Product
                {
                    ProductId = 14,
                    ProductName = "Chobani Strawberry Yoghurt",
                    ProductUnit = "1",
                    ProductPrice = 1.50,
                },

                new Product
                {
                    ProductId = 15,
                    ProductName = "Lurpark Salted Blend",
                    ProductUnit = "250g",
                    ProductPrice = 5.00,
                },

                new Product
                {
                    ProductId = 16,
                    ProductName = "Bega Farmers Tasty",
                    ProductUnit = "250g",
                    ProductPrice = 4.00,
                },

                new Product
                {
                    ProductId = 17,
                    ProductName = "Babybel Mini",
                    ProductUnit = "100g",
                    ProductPrice = 4.20,
                },

                new Product
                {
                    ProductId = 18,
                    ProductName = "Cobram EVOO",
                    ProductUnit = "375ml",
                    ProductPrice = 8.00,
                },

                new Product
                {
                    ProductId = 19,
                    ProductName = "Heinz Tomato Soup",
                    ProductUnit = "535g",
                    ProductPrice = 2.50,
                },

                new Product
                {
                    ProductId = 20,
                    ProductName = "John West Tuna can",
                    ProductUnit = "95g",
                    ProductPrice = 1.50,
                },

                new Product
                {
                    ProductId = 21,
                    ProductName = "Cadbury Dairy Milk",
                    ProductUnit = "200g",
                    ProductPrice = 5.00,
                },

                new Product
                {
                    ProductId = 22,
                    ProductName = "Coca Cola",
                    ProductUnit = "2 litre",
                    ProductPrice = 2.85,
                },

                new Product
                {
                    ProductId = 23,
                    ProductName = "Smith's Original Share Pack Crisps",
                    ProductUnit = "170g",
                    ProductPrice = 3.29,
                },

                new Product
                {
                    ProductId = 24,
                    ProductName = "Birds Eye Fish Fingers",
                    ProductUnit = "375g",
                    ProductPrice = 4.50,
                },

                new Product
                {
                    ProductId = 25,
                    ProductName = "Berri Orange Juice",
                    ProductUnit = "2 litre",
                    ProductPrice = 6.00,
                },

                new Product
                {
                    ProductId = 26,
                    ProductName = "Vegemite",
                    ProductUnit = "380g",
                    ProductPrice = 6.00,
                },

                new Product
                {
                    ProductId = 27,
                    ProductName = "Cheddar Shapes",
                    ProductUnit = "175g",
                    ProductPrice = 2.00,
                },

                new Product
                {
                    ProductId = 28,
                    ProductName = "Colgate Total ToothPaste",
                    ProductUnit = "110g",
                    ProductPrice = 3.50,
                },

                new Product
                {
                    ProductId = 29,
                    ProductName = "Milo Chocolate Malt",
                    ProductUnit = "200g",
                    ProductPrice = 4.00,
                },

                new Product
                {
                    ProductId = 30,
                    ProductName = "Weet Bix Saniatarium Organic",
                    ProductUnit = "750g",
                    ProductPrice = 5.33,
                },

                new Product
                {
                    ProductId = 31,
                    ProductName = "Lindt Excellence 70% Cocoa Block",
                    ProductUnit = "100g",
                    ProductPrice = 4.25,
                },

                new Product
                {
                    ProductId = 32,
                    ProductName = "Original Tim Tams Chocolate",
                    ProductUnit = "200g",
                    ProductPrice = 3.65,
                },

                new Product
                {
                    ProductId = 33,
                    ProductName = "Philadelphia Original Cream Cheese",
                    ProductUnit = "250g",
                    ProductPrice = 4.30,
                },

                new Product
                {
                    ProductId = 34,
                    ProductName = "Moccona Classic Instant Medium Roast",
                    ProductUnit = "100g",
                    ProductPrice = 6.00,
                },

                new Product
                {
                    ProductId = 35,
                    ProductName = "Capilano Sqeezable Honey",
                    ProductUnit = "500g",
                    ProductPrice = 7.35,
                },

                new Product
                {
                    ProductId = 36,
                    ProductName = "Nutella Jar",
                    ProductUnit = "400g",
                    ProductPrice = 4.00,
                },

                new Product
                {
                    ProductId = 37,
                    ProductName = "Arnott's Scotch Finger",
                    ProductUnit = "250g",
                    ProductPrice = 2.85,
                },

                new Product
                {
                    ProductId = 38,
                    ProductName = "South Cape Greek Feta",
                    ProductUnit = "200g",
                    ProductPrice = 5.00,
                },

                new Product
                {
                    ProductId = 39,
                    ProductName = "Sacla Pasta Tomato Basil Sauce",
                    ProductUnit = "420g",
                    ProductPrice = 4.50,
                },

                new Product
                {
                    ProductId = 40,
                    ProductName = "Primo English Ham",
                    ProductUnit = "100g",
                    ProductPrice = 3.00,
                },

                new Product
                {
                    ProductId = 41,
                    ProductName = "Primo Short Cut Rindless Bacon",
                    ProductUnit = "175g",
                    ProductPrice = 5.00,
                },

                new Product
                {
                    ProductId = 42,
                    ProductName = "Golden Circle Pinapple Pieces in natural juice",
                    ProductUnit = "440g",
                    ProductPrice = 3.25,
                },

                new Product
                {
                    ProductId = 43,
                    ProductName = "San Renmo Linguine Pasta No 1",
                    ProductUnit = "500g",
                    ProductPrice = 1.95,
                }
            );
        }
    }
}
