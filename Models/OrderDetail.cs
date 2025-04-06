using System.ComponentModel.DataAnnotations;

namespace Reina.MacCredy.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string SelectedOptions { get; set; } = string.Empty;
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
