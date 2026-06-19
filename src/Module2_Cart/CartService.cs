using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoPartsStoreWPF
{
    public class CartService
    {
        private List<CartItem> items = new List<CartItem>();

        public void AddItem(Product product, int quantity = 1)
        {
            var existing = items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing != null)
                existing.Quantity += quantity;
            else
                items.Add(new CartItem { ProductId = product.Id, Name = product.Name, Price = product.Price, Quantity = quantity });
        }

        public void RemoveItem(int productId)
        {
            var item = items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null) items.Remove(item);
        }

        public void UpdateQuantity(int productId, int newQuantity)
        {
            if (newQuantity <= 0) { RemoveItem(productId); return; }
            var item = items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null) item.Quantity = newQuantity;
        }
        // п
        public decimal GetTotal() => items.Sum(i => i.Total);
        public List<CartItem> GetItems() => items;
        public void Clear() => items.Clear();
        public int GetItemCount() => items.Sum(i => i.Quantity);
    }
}