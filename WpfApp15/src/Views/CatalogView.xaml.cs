using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AutoPartsStoreWPF
{
    public partial class CatalogView : UserControl
    {
        private ProductService productService;
        private CartService cartService;

        public CatalogView(ProductService ps, CartService cs)
        {
            InitializeComponent();
            productService = ps;
            cartService = cs;
            Refresh(null, null);
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = productService.GetAll();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            var results = productService.Search(txtSearch.Text);
            dgProducts.ItemsSource = results;
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            string category = (cmbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (category == "Все") category = null;

            decimal? min = decimal.TryParse(txtMinPrice.Text, out decimal minVal) ? minVal : (decimal?)null;
            decimal? max = decimal.TryParse(txtMaxPrice.Text, out decimal maxVal) ? maxVal : (decimal?)null;

            var results = productService.Filter(category, min, max);
            dgProducts.ItemsSource = results;
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            var product = dgProducts.SelectedItem as Product;
            if (product != null)
            {
                cartService.AddItem(product);
                MessageBox.Show($"Товар '{product.Name}' добавлен в корзину!", "Успешно",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите товар!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}