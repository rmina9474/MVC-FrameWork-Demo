using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Reina.MacCredy.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [Required]
        [StringLength(500)]
        public required string Comment { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Required]
        public required string UserId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [ForeignKey("UserId")]
        [ValidateNever]
        public required ApplicationUser User { get; set; }
        
        [ForeignKey("ProductId")]
        [ValidateNever]
        public required Product Product { get; set; }
    }
}