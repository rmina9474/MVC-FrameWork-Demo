using System.Collections.Generic;

namespace Reina.MacCredy.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        
        // Add a TotalPrice property for JSON serialization
        public decimal TotalPrice => GetTotal();

        public void AddItem(Product product, int quantity, decimal customPrice = 0, string selectedOptions = "")
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id && 
                                                        (i.SelectedOptions == selectedOptions || 
                                                         (string.IsNullOrEmpty(i.SelectedOptions) && string.IsNullOrEmpty(selectedOptions))));
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = customPrice > 0 ? customPrice : product.Price,
                    Quantity = quantity,
                    SelectedOptions = selectedOptions
                });
            }
        }

        public void RemoveItem(int productId, string selectedOptions = "")
        {
            if (string.IsNullOrEmpty(selectedOptions))
            {
                // Remove all items with this product ID regardless of options
                Items.RemoveAll(i => i.ProductId == productId);
            }
            else
            {
                // Remove the specific product with these options
                var item = Items.FirstOrDefault(i => i.ProductId == productId && i.SelectedOptions == selectedOptions);
                if (item != null)
                {
                    Items.Remove(item);
                }
            }
        }

        public void Clear()
        {
            Items.Clear();
        }

        public decimal GetTotal()
        {
            return Items.Sum(i => i.Price * i.Quantity);
        }
    }
}
