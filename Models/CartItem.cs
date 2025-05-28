using System;

namespace Music_Shopping.Models
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }

        public decimal TotalPrice
        {
            get { return Price * Quantity; }
        }
    }
} 