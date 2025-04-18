﻿using System.ComponentModel.DataAnnotations;

namespace Reina.MacCredy.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
