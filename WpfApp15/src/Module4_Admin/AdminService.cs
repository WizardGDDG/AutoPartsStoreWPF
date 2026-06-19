using System.Collections.Generic;

namespace AutoPartsStoreWPF
{
    public class AdminService
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public AdminService(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        public List<Order> GetAllOrders() => _orderService.GetAllOrders();
        public void UpdateOrderStatus(int orderId, string status) => _orderService.UpdateOrderStatus(orderId, status);
        public List<Product> GetAllProducts() => _productService.GetAll();
        public Product GetProductById(int id) => _productService.GetById(id);
        public void AddProduct(Product product) => _productService.Add(product);
        public void UpdateProduct(Product product) => _productService.Update(product);
        public void DeleteProduct(int id) => _productService.Delete(id);
    }
}