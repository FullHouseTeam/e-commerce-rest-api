using Microsoft.EntityFrameworkCore;
using System;

namespace YourNamespace.DbContexts
{
    public class ProductLibraryContext : DbContext
    {
        public ProductLibraryContext(DbContextOptions<ProductLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed the database with dummy data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Description for Product 1",
                    Price = 19.99m,
                    Category = "Electronics"
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "Description for Product 2",
                    Price = 29.99m,
                    Category = "Clothing"
                },
                new Product
                {
                    Id = 3,
                    Name = "Product 3",
                    Description = "Description for Product 3",
                    Price = 9.99m,
                    Category = "Home & Kitchen"
                },
                new Product
                {
                    Id = 4,
                    Name = "Product 4",
                    Description = "Description for Product 4",
                    Price = 49.99m,
                    Category = "Toys"
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
