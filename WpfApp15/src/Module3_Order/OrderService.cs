using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AutoPartsStoreWPF
{
    public class OrderService : IOrderService
    {
        private List<Order> orders;
        private readonly string dataFile;
        private int nextId;

        public OrderService()
        {
            orders = new List<Order>();
            dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "orders.json");
            nextId = 1;
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                if (File.Exists(dataFile))
                {
                    string json = File.ReadAllText(dataFile);
                    orders = JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();

                    if (orders.Count > 0)
                    {
                        nextId = orders.Max(o => o.Id) + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                orders = new List<Order>();
            }
        }

        private void SaveOrders()
        {
            try
            {
                string directory = Path.GetDirectoryName(dataFile);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(orders, options);
                File.WriteAllText(dataFile, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка сохранения заказов: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.Now:yyyyMMdd}-{nextId:D3}";
        }

        public Order CreateOrder(CustomerInfo customer, List<CartItem> items)
        {
            
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Данные покупателя не заполнены");
            }

            if (items == null || items.Count == 0)
            {
                throw new ArgumentException("Корзина пуста", nameof(items));
            }

            
            var order = new Order
            {
                Id = nextId,
                OrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.Now,
                Customer = new CustomerInfo
                {
                    LastName = customer.LastName ?? "",
                    FirstName = customer.FirstName ?? "",
                    Address = customer.Address ?? "",
                    Phone = customer.Phone ?? "",
                    Email = customer.Email ?? ""
                },
                Items = new List<CartItem>(items),
                TotalAmount = items.Sum(i => i.Total),
                Status = "Новый"
            };

            
            nextId++;

            
            orders.Add(order);
            SaveOrders();

            return order;
        }

        public List<Order> GetAllOrders()
        {
            return orders;
        }

        public Order GetOrderById(int id)
        {
            return orders.FirstOrDefault(o => o.Id == id);
        }

        public void UpdateOrderStatus(int id, string status)
        {
            var order = GetOrderById(id);
            if (order != null)
            {
                order.Status = status;
                SaveOrders();
            }
        }
    }
}