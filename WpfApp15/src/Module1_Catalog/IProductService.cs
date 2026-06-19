using System.Collections.Generic;

namespace AutoPartsStoreWPF
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(int id);
        List<Product> Search(string query);
        List<Product> Filter(string category, decimal? minPrice, decimal? maxPrice);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}