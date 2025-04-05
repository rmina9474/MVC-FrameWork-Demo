﻿using System.ComponentModel.DataAnnotations;

namespace Reina.MacCredy.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public required Order Order { get; set; }
        public required Product Product { get; set; }
    }
}
