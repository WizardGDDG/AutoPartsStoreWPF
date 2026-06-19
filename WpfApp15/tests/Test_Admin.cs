using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace AutoPartsStoreWPF.Tests
{
    public class Test_Admin
    {
        [Fact]
        public void Test_AddProduct_ShouldIncreaseCount()
        {
            var productService = new ProductService();
            var orderService = new OrderService();
            var admin = new AdminService(productService, orderService);

            var oldCount = admin.GetAllProducts().Count;
            var product = new Product
            {
                Name = "Новый товар",
                Category = "Тест",
                Price = 100,
                Stock = 5
            };

            admin.AddProduct(product);

            Assert.Equal(oldCount + 1, admin.GetAllProducts().Count);
        }

        [Fact]
        public void Test_GetProductById_ShouldReturnCorrectProduct()
        {
            var productService = new ProductService();
            var orderService = new OrderService();
            var admin = new AdminService(productService, orderService);

            var product = new Product
            {
                Name = "Тестовый товар",
                Category = "Тест",
                Price = 100,
                Stock = 5
            };
            admin.AddProduct(product);
            var added = admin.GetAllProducts().Last();

            var found = admin.GetProductById(added.Id);

            Assert.NotNull(found);
            Assert.Equal(added.Id, found.Id);
        }

        [Fact]
        public void Test_UpdateProduct_ShouldChangeData()
        {
            var productService = new ProductService();
            var orderService = new OrderService();
            var admin = new AdminService(productService, orderService);

            var product = new Product
            {
                Name = "Для обновления",
                Category = "Тест",
                Price = 100,
                Stock = 5
            };
            admin.AddProduct(product);
            var added = admin.GetAllProducts().Last();

            added.Name = "Обновленное имя";
            admin.UpdateProduct(added);
            var updated = admin.GetProductById(added.Id);

            Assert.Equal("Обновленное имя", updated.Name);
        }

        [Fact]
        public void Test_DeleteProduct_ShouldDecreaseCount()
        {
            var productService = new ProductService();
            var orderService = new OrderService();
            var admin = new AdminService(productService, orderService);

            var product = new Product
            {
                Name = "Для удаления",
                Category = "Тест",
                Price = 100,
                Stock = 5
            };
            admin.AddProduct(product);
            var added = admin.GetAllProducts().Last();

            var oldCount = admin.GetAllProducts().Count;

            admin.DeleteProduct(added.Id);

            Assert.Equal(oldCount - 1, admin.GetAllProducts().Count);
        }

        [Fact]
        public void Test_UpdateOrderStatus_ShouldChangeStatus()
        {
            var productService = new ProductService();
            var orderService = new OrderService();
            var admin = new AdminService(productService, orderService);

            var customer = new CustomerInfo { LastName = "Тест", FirstName = "Тест" };
            var items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Тест", Price = 100, Quantity = 1 }
            };
            var order = orderService.CreateOrder(customer, items);

            admin.UpdateOrderStatus(order.Id, "В обработке");
            var updated = orderService.GetOrderById(order.Id);

            Assert.Equal("В обработке", updated.Status);
        }
    }
}