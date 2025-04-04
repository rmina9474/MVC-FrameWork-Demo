using System.ComponentModel.DataAnnotations;

namespace Reina.MacCredy.Models
{
    public class ProductOption
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public decimal AdditionalPrice { get; set; } = 0;
        
        public string? Description { get; set; }
        
        public int ProductId { get; set; }
        
        public Product? Product { get; set; }
        
        public string OptionType { get; set; } = "Size"; // Size, Milk, Syrup, Extra, etc.
        
        public bool IsDefault { get; set; } = false;
    }
} 