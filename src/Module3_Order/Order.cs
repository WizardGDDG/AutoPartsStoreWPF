using System;
using System.Collections.Generic;

namespace AutoPartsStoreWPF
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public CustomerInfo Customer { get; set; } = new CustomerInfo();
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Новый";
    }
}