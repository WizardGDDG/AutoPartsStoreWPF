using System.Collections.Generic;

namespace AutoPartsStoreWPF
{
    public interface IOrderService
    {
        Order CreateOrder(CustomerInfo customer, List<CartItem> items);
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        void UpdateOrderStatus(int id, string status);
        string GenerateOrderNumber();
    }
}