using Xunit;
using System.Linq;

namespace AutoPartsStoreWPF.Tests
{
    public class Test_Catalog
    {
        [Fact]
        public void Test_GetAll_ShouldReturnProducts()
        {
            var service = new ProductService();
            var products = service.GetAll();

            Assert.NotNull(products);
            Assert.True(products.Count > 0);
        }

        [Fact]
        public void Test_GetById_ShouldReturnCorrectProduct()
        {
            var service = new ProductService();
            var product = service.GetById(1);

            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
        }

        [Fact]
        public void Test_Search_ShouldFindProduct()
        {
            var service = new ProductService();
            var results = service.Search("поршневая");

            Assert.True(results.Count > 0);
        }

        [Fact]
        public void Test_Search_WithEmptyQuery_ShouldReturnAll()
        {
            var service = new ProductService();
            var all = service.GetAll();
            var results = service.Search("");

            Assert.Equal(all.Count, results.Count);
        }

        [Fact]
        public void Test_Filter_ByCategory()
        {
            var service = new ProductService();
            var results = service.Filter("Электрика", null, null);

            Assert.All(results, p => Assert.Equal("Электрика", p.Category));
        }

        [Fact]
        public void Test_Filter_ByPriceRange()
        {
            var service = new ProductService();
            var results = service.Filter(null, 5000, 10000);

            Assert.All(results, p =>
            {
                Assert.True(p.Price >= 5000);
                Assert.True(p.Price <= 10000);
            });
        }

        [Fact]
        public void Test_AddProduct_ShouldIncreaseCount()
        {
            var service = new ProductService();
            var oldCount = service.GetAll().Count;

            var product = new Product
            {
                Name = "Тестовый товар",
                Category = "Тест",
                Price = 1000,
                Stock = 5
            };

            service.Add(product);
            var newCount = service.GetAll().Count;

            Assert.Equal(oldCount + 1, newCount);
        }

        [Fact]
        public void Test_UpdateProduct_ShouldChangeData()
        {
            var service = new ProductService();
            var product = new Product
            {
                Name = "Для обновления",
                Category = "Тест",
                Price = 1000,
                Stock = 5
            };
            service.Add(product);
            var added = service.GetAll().Last();

            added.Name = "Обновленное имя";
            service.Update(added);
            var updated = service.GetById(added.Id);

            Assert.Equal("Обновленное имя", updated.Name);
        }

        [Fact]
        public void Test_DeleteProduct_ShouldDecreaseCount()
        {
            var service = new ProductService();
            var product = new Product
            {
                Name = "Для удаления",
                Category = "Тест",
                Price = 1000,
                Stock = 5
            };
            service.Add(product);
            var added = service.GetAll().Last();
            var oldCount = service.GetAll().Count;

            service.Delete(added.Id);
            var newCount = service.GetAll().Count;

            Assert.Equal(oldCount - 1, newCount);
        }
    }
}