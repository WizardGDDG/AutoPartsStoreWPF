using Xunit;
using System.Linq;

namespace AutoPartsStoreWPF.Tests
{
    public class Test_Cart
    {
        [Fact]
        public void Test_AddItem_ShouldIncreaseTotal()
        {
            var cart = new CartService();
            var product = new Product { Id = 1, Name = "Тест", Price = 100, Stock = 10 };

            cart.AddItem(product, 2);

            Assert.Equal(200, cart.GetTotal());
            Assert.Equal(2, cart.GetItemCount());
        }

        [Fact]
        public void Test_AddItem_ShouldAddNewItem()
        {
            var cart = new CartService();
            var product = new Product { Id = 1, Name = "Тест", Price = 100, Stock = 10 };

            cart.AddItem(product, 2);
            var items = cart.GetItems();

            Assert.Single(items);
            Assert.Equal(1, items[0].ProductId);
            Assert.Equal(2, items[0].Quantity);
        }

        [Fact]
        public void Test_AddItem_WithExistingItem_ShouldIncreaseQuantity()
        {
            var cart = new CartService();
            var product = new Product { Id = 1, Name = "Тест", Price = 100, Stock = 10 };

            cart.AddItem(product, 2);
            cart.AddItem(product, 3);

            var items = cart.GetItems();
            Assert.Single(items);
            Assert.Equal(5, items[0].Quantity);
            Assert.Equal(500, cart.GetTotal());
        }

        [Fact]
        public void Test_RemoveItem_ShouldDecreaseCount()
        {
            var cart = new CartService();
            var product = new Product { Id = 1, Name = "Тест", Price = 100, Stock = 10 };

            cart.AddItem(product, 2);
            cart.RemoveItem(1);

            Assert.Equal(0, cart.GetItemCount());
            Assert.Empty(cart.GetItems());
        }

        [Fact]
        public void Test_UpdateQuantity_ShouldChangeQuantity()
        {
            var cart = new CartService();
            var product = new Product { Id = 1, Name = "Тест", Price = 100, Stock = 10 };

            cart.AddItem(product, 2);
            cart.UpdateQuantity(1, 5);

            var items = cart.GetItems();
            Assert.Single(items);
            Assert.Equal(5, items[0].Quantity);
            Assert.Equal(500, cart.GetTotal());
        }

        [Fact]
        public void Test_UpdateQuantity_WithZero_ShouldRemoveItem()
        {
            var cart = new CartService();
            var product = new Product { Id = 1, Name = "Тест", Price = 100, Stock = 10 };

            cart.AddItem(product, 2);
            cart.UpdateQuantity(1, 0);

            Assert.Empty(cart.GetItems());
        }

        [Fact]
        public void Test_Clear_ShouldEmptyCart()
        {
            var cart = new CartService();

            cart.AddItem(new Product { Id = 1, Name = "Тест1", Price = 100, Stock = 10 }, 2);
            cart.AddItem(new Product { Id = 2, Name = "Тест2", Price = 200, Stock = 5 }, 1);
            cart.Clear();

            Assert.Empty(cart.GetItems());
            Assert.Equal(0, cart.GetTotal());
        }

        [Fact]
        public void Test_GetTotal_ShouldCalculateCorrectly()
        {
            var cart = new CartService();

            cart.AddItem(new Product { Id = 1, Name = "Тест1", Price = 100, Stock = 10 }, 2);
            cart.AddItem(new Product { Id = 2, Name = "Тест2", Price = 50, Stock = 5 }, 3);

            Assert.Equal(350, cart.GetTotal());
        }
    }
}