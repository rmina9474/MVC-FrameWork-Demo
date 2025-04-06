using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;    
using Reina.MacCredy.Models;

namespace Reina.MacCredy.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure decimal properties for SQLite
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasConversion<double>();
                
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Price)
                .HasConversion<double>();
                
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasConversion<double>();
                
            modelBuilder.Entity<ProductOption>()
                .Property(po => po.AdditionalPrice)
                .HasConversion<double>();
                
            // Configure Order to use UserId as ForeignKey for ApplicationUser
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ApplicationUser)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
