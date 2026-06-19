using System.Windows;
using System.Windows.Controls;

namespace AutoPartsStoreWPF
{
    public partial class CartView : UserControl
    {
        private CartService cartService;

        public CartView(CartService cs)
        {
            InitializeComponent();
            cartService = cs;
            Refresh(null, null);
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            dgCart.ItemsSource = null;
            dgCart.ItemsSource = cartService.GetItems();
            txtTotal.Text = cartService.GetTotal().ToString("C");
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            var item = dgCart.SelectedItem as CartItem;
            if (item != null)
            {
                cartService.RemoveItem(item.ProductId);
                Refresh(null, null);
            }
        }

        private void ClearCart(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Очистить корзину?", "Подтверждение",
                               MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                cartService.Clear();
                Refresh(null, null);
            }
        }
    }
}