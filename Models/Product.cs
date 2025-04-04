using System.ComponentModel.DataAnnotations;
namespace Reina.MacCredy.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public required string Name { get; set; }
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductImage>? Images { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        
        // Add reference to product reviews
        public List<ProductReview>? Reviews { get; set; }
        
        // Helper method to calculate average rating
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public double AverageRating => Reviews != null && Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;
    }
}
