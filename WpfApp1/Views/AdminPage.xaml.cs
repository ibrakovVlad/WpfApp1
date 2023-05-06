using Market.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Market.Views
{
    public partial class AdminPage : Page
    {
        private readonly MyDbContext _dbContext;
        private Product _selectedProduct;

        public AdminPage()
        {
            InitializeComponent();
            _dbContext = MyDbContext.GetContext();
            UpdateProducts();
        }

        private void DoGoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void OpenAddProduct(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductWindow();
            if (addProductWindow.ShowDialog() == true)
            {
                UpdateProducts();
            }
        }

        private void OpenEditProduct(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct == null)
            {
                MessageBox.Show("Ошибка, продукт не выбран");
                return;
            }

            var editProductWindow = new EditProductWindow(_selectedProduct);
            if (editProductWindow.ShowDialog() == true)
            {
                UpdateProducts();
            }
        }

        private void RemoveProduct(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct == null)
            {
                MessageBox.Show("Ошибка, продукт не выбран");
                return;
            }

            _dbContext.Products.Remove(_selectedProduct);
            _dbContext.SaveChanges();
            UpdateProducts();
        }

        private void UpdateProducts()
        {
            ProductsList.ItemsSource = _dbContext.Products
                .ToList();
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedProduct = (Product)ProductsList.SelectedItem;
        }
    }
}
