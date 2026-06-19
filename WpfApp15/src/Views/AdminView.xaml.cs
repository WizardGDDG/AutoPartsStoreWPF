using System.Windows;
using System.Windows.Controls;

namespace AutoPartsStoreWPF
{
    public partial class AdminView : UserControl
    {
        private AdminService adminService;

        public AdminView(AdminService service)
        {
            InitializeComponent();
            adminService = service;
            Refresh(null, null);
            RefreshOrders(null, null);
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = null;
            dgProducts.ItemsSource = adminService.GetAllProducts();
        }

        private void RefreshOrders(object sender, RoutedEventArgs e)
        {
            dgOrders.ItemsSource = null;
            dgOrders.ItemsSource = adminService.GetAllOrders();
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            var product = new Product { Name = "Новый товар", Category = "Детали двигателя", Price = 1000, Stock = 10 };
            adminService.AddProduct(product);
            Refresh(null, null);
            MessageBox.Show("Товар добавлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateProduct(object sender, RoutedEventArgs e)
        {
            var product = dgProducts.SelectedItem as Product;
            if (product != null)
            {
                adminService.UpdateProduct(product);
                Refresh(null, null);
                MessageBox.Show("Товар обновлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteProduct(object sender, RoutedEventArgs e)
        {
            var product = dgProducts.SelectedItem as Product;
            if (product != null && MessageBox.Show($"Удалить '{product.Name}'?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                adminService.DeleteProduct(product.Id);
                Refresh(null, null);
                MessageBox.Show("Товар удален!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateStatus(object sender, RoutedEventArgs e)
        {
            var order = dgOrders.SelectedItem as Order;
            if (order != null)
            {
                string status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
                adminService.UpdateOrderStatus(order.Id, status);
                RefreshOrders(null, null);
                MessageBox.Show("Статус обновлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}