using System.ComponentModel.DataAnnotations;

namespace Reina.MacCredy.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public required string Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public List<Product>? Products { get; set; }
    }
}
