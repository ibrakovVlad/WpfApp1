using Market.Services;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Market.Views
{
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            UpdateList();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void UpdateList()
        {
            ProductsList.ItemsSource = CartService.Products.ToList();
        }

        private void ClearCart(object sender, RoutedEventArgs e)
        {
            CartService.Clear();
            UpdateList();
            MessageBox.Show("Корзина очищена успешно");
        }

        private void Buy(object sender, RoutedEventArgs e)
        {
            CartService.Clear();
            UpdateList();
            MessageBox.Show("Покупка соверешна успешно");
        }
    }
}
