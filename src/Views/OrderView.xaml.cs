using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AutoPartsStoreWPF
{
    public partial class OrderView : UserControl
    {
        private OrderService orderService;
        private CartService cartService;

        public OrderView(OrderService os, CartService cs)
        {
            InitializeComponent();
            orderService = os;
            cartService = cs;
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            txtTotal.Text = $"Сумма заказа: {cartService.GetTotal():C}";
        }

        private void SubmitOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                var items = cartService.GetItems();
                if (items.Count == 0)
                {
                    MessageBox.Show("Корзина пуста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLastName.Text) || string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    MessageBox.Show("Заполните имя и фамилию!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var customer = new CustomerInfo
                {
                    LastName = txtLastName.Text,
                    FirstName = txtFirstName.Text,
                    Address = txtAddress.Text,
                    Phone = txtPhone.Text,
                    Email = txtEmail.Text
                };

                var order = orderService.CreateOrder(customer, items);
                cartService.Clear();

                txtResult.Text = $"Заказ #{order.OrderNumber} оформлен!\nСумма: {order.TotalAmount:C}\nСтатус: {order.Status}";

                MessageBox.Show($"Заказ #{order.OrderNumber} успешно оформлен!\nСумма: {order.TotalAmount:C}",
                               "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                UpdateTotal();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}