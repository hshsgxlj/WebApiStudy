using Microsoft.EntityFrameworkCore;
using WebApiStudy.Models;

namespace WebApiStudy.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Shirt> Shirts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //data seading
            modelBuilder.Entity<Shirt>().HasData(
                new Shirt { ShirtId = 1, Brand = "Nike", Color = "Red", Gender = "men", Size = 9, Price = 29.99 },
                new Shirt { ShirtId = 2, Brand = "Adidas", Color = "Blue", Gender = "women", Size = 6, Price = 25.99 },
                new Shirt { ShirtId = 3, Brand = "Puma", Color = "Black", Gender = "men", Size = 10, Price = 19.99 },
                new Shirt { ShirtId = 4, Brand = "Uniqlo", Color = "White", Gender = "women", Size = 7, Price = 15.99 }
             );
        }
    }
}
