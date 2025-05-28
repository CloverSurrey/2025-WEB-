using System;
using System.Collections.Generic;
using System.Linq;

namespace Music_Shopping.Models
{
    public class ShoppingCart
    {
        private List<CartItem> _items = new List<CartItem>();

        public List<CartItem> Items
        {
            get { return _items; }
        }

        public void AddItem(IProduct product, int quantity)
        {
            var existingItem = _items.FirstOrDefault(i => i.ProductId == product.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price ?? 0,
                    Quantity = quantity,
                    Type = product.Type
                });
            }
        }

        public void RemoveItem(string productId)
        {
            _items.RemoveAll(i => i.ProductId == productId);
        }

        public void UpdateQuantity(string productId, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }

        public decimal GetTotal()
        {
            return _items.Sum(i => i.TotalPrice);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}