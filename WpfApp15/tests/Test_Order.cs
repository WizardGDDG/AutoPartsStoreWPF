using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AutoPartsStoreWPF.Tests
{
    public class Test_Order
    {
        [Fact]
        public void Test_CreateOrder_ShouldGenerateNumber()
        {
            var service = new OrderService();
            var customer = new CustomerInfo
            {
                LastName = "Иванов",
                FirstName = "Иван"
            };
            var items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Тест", Price = 100, Quantity = 2 }
            };

            var order = service.CreateOrder(customer, items);

            Assert.NotNull(order.OrderNumber);
            Assert.StartsWith("ORD-", order.OrderNumber);
            Assert.Equal("Новый", order.Status);
            Assert.Equal(200, order.TotalAmount);
        }

        [Fact]
        public void Test_CreateOrder_ShouldAddToList()
        {
            var service = new OrderService();
            var oldCount = service.GetAllOrders().Count;
            var customer = new CustomerInfo { LastName = "Тест", FirstName = "Тест" };
            var items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Тест", Price = 100, Quantity = 1 }
            };

            service.CreateOrder(customer, items);
            var newCount = service.GetAllOrders().Count;

            Assert.Equal(oldCount + 1, newCount);
        }

        [Fact]
        public void Test_GetOrderById_ShouldReturnCorrectOrder()
        {
            var service = new OrderService();
            var customer = new CustomerInfo { LastName = "Тест", FirstName = "Тест" };
            var items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Тест", Price = 100, Quantity = 1 }
            };
            var created = service.CreateOrder(customer, items);

            var found = service.GetOrderById(created.Id);

            Assert.NotNull(found);
            Assert.Equal(created.Id, found.Id);
            Assert.Equal(created.OrderNumber, found.OrderNumber);
        }

        [Fact]
        public void Test_UpdateOrderStatus_ShouldChangeStatus()
        {
            var service = new OrderService();
            var customer = new CustomerInfo { LastName = "Тест", FirstName = "Тест" };
            var items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Тест", Price = 100, Quantity = 1 }
            };
            var order = service.CreateOrder(customer, items);

            service.UpdateOrderStatus(order.Id, "Доставлен");
            var updated = service.GetOrderById(order.Id);

            Assert.Equal("Доставлен", updated.Status);
        }

        [Fact]
        public void Test_GenerateOrderNumber_ShouldReturnUniqueNumber()
        {
            var service = new OrderService();

            var number1 = service.GenerateOrderNumber();
            var number2 = service.GenerateOrderNumber();

            Assert.NotEqual(number1, number2);
        }

        [Fact]
        public void Test_CreateOrder_WithEmptyCart_ShouldThrowException()
        {
            var service = new OrderService();
            var customer = new CustomerInfo { LastName = "Тест", FirstName = "Тест" };
            var items = new List<CartItem>();

            Assert.Throws<ArgumentException>(() => service.CreateOrder(customer, items));
        }

        [Fact]
        public void Test_CreateOrder_WithNullCustomer_ShouldThrowException()
        {
            var service = new OrderService();
            var items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Тест", Price = 100, Quantity = 1 }
            };

            Assert.Throws<ArgumentNullException>(() => service.CreateOrder(null, items));
        }
    }
}