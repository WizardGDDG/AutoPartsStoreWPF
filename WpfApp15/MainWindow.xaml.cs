using System.Windows;
using System.Windows.Controls;

namespace AutoPartsStoreWPF
{
    public partial class MainWindow : Window
    {
        private ProductService productService = new ProductService();
        private OrderService orderService = new OrderService();
        private CartService cartService = new CartService();
        private AdminService adminService;

        public MainWindow()
        {
            InitializeComponent();
            adminService = new AdminService(productService, orderService);
            ShowCatalog(null, null);
        }

        private void ShowCatalog(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CatalogView(productService, cartService);
        }

        private void ShowCart(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CartView(cartService);
        }

        private void ShowOrder(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new OrderView(orderService, cartService);
        }

        private void ShowAdmin(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AdminView(adminService);
        }
    }
}