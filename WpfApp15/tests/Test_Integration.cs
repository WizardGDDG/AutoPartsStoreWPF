using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace AutoPartsStoreWPF.Tests
{
    public class Test_Integration
    {
        [Fact]
        public void Test_FullWorkflow_FromCatalogToOrder()
        {
            // 1. Каталог
            var productService = new ProductService();
            var products = productService.GetAll();
            Assert.True(products.Count > 0);

            // 2. Корзина
            var cart = new CartService();
            var product = productService.GetById(1);
            cart.AddItem(product, 2);

            // 3. Заказ
            var orderService = new OrderService();
            var customer = new CustomerInfo
            {
                LastName = "Петров",
                FirstName = "Петр"
            };
            var order = orderService.CreateOrder(customer, cart.GetItems());

            Assert.NotNull(order.OrderNumber);
            Assert.Equal(cart.GetTotal(), order.TotalAmount);

            // 4. Админка - меняем статус
            var admin = new AdminService(productService, orderService);
            admin.UpdateOrderStatus(order.Id, "Доставлен");
            var updatedOrder = orderService.GetOrderById(order.Id);
            Assert.Equal("Доставлен", updatedOrder.Status);
        }

        [Fact]
        public void Test_AddProductToCart_AndCreateOrder()
        {
            var productService = new ProductService();
            var orderService = new OrderService();
            var admin = new AdminService(productService, orderService);

            var newProduct = new Product
            {
                Name = "Интеграционный тест",
                Category = "Тест",
                Price = 500,
                Stock = 10
            };
            admin.AddProduct(newProduct);
            var added = productService.GetAll().Last();

            var cart = new CartService();
            cart.AddItem(added, 3);
            Assert.Equal(1500, cart.GetTotal());

            var customer = new CustomerInfo
            {
                LastName = "Тестов",
                FirstName = "Тест"
            };
            var order = orderService.CreateOrder(customer, cart.GetItems());

            Assert.Equal(1500, order.TotalAmount);
            Assert.Contains(order.Items, i => i.Name == "Интеграционный тест");
        }

        [Fact]
        public void Test_SearchProduct_ThenAddToCart()
        {
            var productService = new ProductService();

            var results = productService.Search("поршневая");
            Assert.True(results.Count > 0);

            var foundProduct = results[0];

            var cart = new CartService();
            cart.AddItem(foundProduct, 1);

            var cartItems = cart.GetItems();
            Assert.Single(cartItems);
            Assert.Equal(foundProduct.Id, cartItems[0].ProductId);
        }

        [Fact]
        public void Test_FilterProducts_AndCreateOrder()
        {
            var productService = new ProductService();

            var filtered = productService.Filter("Электрика", null, null);
            Assert.True(filtered.Count > 0);

            var cart = new CartService();
            foreach (var product in filtered.Take(2))
            {
                cart.AddItem(product, 1);
            }

            var orderService = new OrderService();
            var customer = new CustomerInfo { LastName = "Тест", FirstName = "Тест" };
            var order = orderService.CreateOrder(customer, cart.GetItems());

            Assert.NotNull(order);
            Assert.Equal(cart.GetTotal(), order.TotalAmount);
        }

        [Fact]
        public void Test_MultipleOrders_FromDifferentCustomers()
        {
            var productService = new ProductService();
            var orderService = new OrderService();
            var admin = new AdminService(productService, orderService);

            var customer1 = new CustomerInfo { LastName = "Иванов", FirstName = "Иван" };
            var customer2 = new CustomerInfo { LastName = "Петров", FirstName = "Петр" };

            var items1 = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Товар1", Price = 100, Quantity = 2 }
            };
            var items2 = new List<CartItem>
            {
                new CartItem { ProductId = 2, Name = "Товар2", Price = 200, Quantity = 1 }
            };

            var order1 = orderService.CreateOrder(customer1, items1);
            var order2 = orderService.CreateOrder(customer2, items2);

            var orders = admin.GetAllOrders();
            Assert.Contains(orders, o => o.OrderNumber == order1.OrderNumber);
            Assert.Contains(orders, o => o.OrderNumber == order2.OrderNumber);
            Assert.NotEqual(order1.OrderNumber, order2.OrderNumber);
        }
    }
}