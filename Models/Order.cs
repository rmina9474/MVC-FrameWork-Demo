using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Reina.MacCredy.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        
        [Required(ErrorMessage = "Shipping address is required")]
        public string ShippingAddress { get; set; } = string.Empty;
        
        public string Notes { get; set; } = "No additional notes.";
        
        // Fields for guest checkout
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;
        
        [Display(Name = "State/Province")]
        public string State { get; set; } = string.Empty;
        
        [Display(Name = "Postal Code")]
        public string ZipCode { get; set; } = string.Empty;
        
        // Flag to identify guest orders
        public bool IsGuestOrder { get; set; }
        
        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }
        
        [ValidateNever]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
