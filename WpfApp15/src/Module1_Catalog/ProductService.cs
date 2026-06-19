using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AutoPartsStoreWPF
{
    public class ProductService : IProductService
    {
        private List<Product> products = new List<Product>();
        
        private string dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "catalog.json");

        public ProductService()
        {
            LoadData();
        }
        // r
        private void LoadData()
        {
            try
            {
                if (File.Exists(dataFile))
                {
                    string json = File.ReadAllText(dataFile);
                    products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
                }
                else
                {
                    
                    products = new List<Product>
                    {
                        new Product { Id = 1, Name = "Поршневая группа 1.8", Category = "Детали двигателя", Price = 15000, Stock = 10 },
                        new Product { Id = 2, Name = "Амортизатор передний", Category = "Ходовая часть", Price = 8500, Stock = 5 },
                        new Product { Id = 3, Name = "Стартер 1.4", Category = "Электрика", Price = 12000, Stock = 3 },
                        new Product { Id = 4, Name = "Масло моторное 5W-40", Category = "Расходные материалы", Price = 3500, Stock = 20 },
                        new Product { Id = 5, Name = "Ремень ГРМ", Category = "Детали двигателя", Price = 5000, Stock = 7 }
                    };
                    SaveData();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                products = new List<Product>();
            }
        }

        private void SaveData()
        {
            try
            {
                
                string directory = Path.GetDirectoryName(dataFile);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(products, options);
                File.WriteAllText(dataFile, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        public List<Product> GetAll() => products;
        public Product GetById(int id) => products.FirstOrDefault(p => p.Id == id);

        public List<Product> Search(string query)
        {
            if (string.IsNullOrEmpty(query)) return products;
            return products.Where(p => p.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        public List<Product> Filter(string category, decimal? minPrice, decimal? maxPrice)
        {
            var result = products.AsEnumerable();
            if (!string.IsNullOrEmpty(category))
                result = result.Where(p => p.Category == category);
            if (minPrice.HasValue)
                result = result.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                result = result.Where(p => p.Price <= maxPrice.Value);
            return result.ToList();
        }

        public void Add(Product product)
        {
            product.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
            products.Add(product);
            SaveData();
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Category = product.Category;
                existing.Price = product.Price;
                existing.Stock = product.Stock;
                existing.Description = product.Description;
                SaveData();
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                products.Remove(product);
                SaveData();
            }
        }
    }
}