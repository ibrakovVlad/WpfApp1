using Market.Models;
using Market.Services;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Views;

namespace Market.Views
{
    public partial class BuyProductsPage : Page
    {
        private readonly MyDbContext _dbContext;

        public BuyProductsPage()
        {
            InitializeComponent();
            _dbContext = MyDbContext.GetContext();
            UpdateProducts();

            if (UserService.User.Login == "admin" || UserService.User.Email == "admin@gmail.com")
            {
                UsersButton.Visibility = Visibility.Visible;
                AdminPanelButton.Visibility = Visibility.Visible;
            }
            else
            {
                UsersButton.Visibility = Visibility.Hidden;
                AdminPanelButton.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cartPage = new CartPage();
            NavigationService.Navigate(cartPage);
        }

        private void OpenPersonalAccount(object sender, RoutedEventArgs e)
        {
            var personalAccountPage = new UsersPage();
            NavigationService.Navigate(personalAccountPage);
        }

        private void OpenAdminPage(object sender, RoutedEventArgs e)
        {
            var adminPage = new AdminPage();
            NavigationService.Navigate(adminPage);
        }

        private void UpdateProducts()
        {
            ProductsList.ItemsSource = _dbContext.Products
                .ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            var grid = (Grid)sender;
            var children = ((Grid)((StackPanel)grid.Children[0]).Children[0]).Children[1];
            children.Visibility = Visibility.Visible;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            var grid = (Grid)sender;
            var children = ((Grid)((StackPanel)grid.Children[0]).Children[0]).Children[1];
            children.Visibility = Visibility.Hidden;
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            var clickedButton = (Button)sender;
            var stackPanel = (StackPanel)clickedButton.Parent;
            var countBox = (TextBox)stackPanel.Children[3];
            var idBlock = (TextBlock)stackPanel.Children[5];
            var gridImage = (Grid)stackPanel.Children[0];
            var gridSizes = (Grid)gridImage.Children[1];
            var wrapPanelSizes = (WrapPanel)gridSizes.Children[0];

            string size = null;
            foreach (RadioButton radio in wrapPanelSizes.Children)
            {
                if (radio.IsChecked == true)
                {
                    size = radio.Content.ToString();
                }
            }

            if (size == null)
            {
                MessageBox.Show("Размер не указан");
                return;
            }

            if (int.TryParse(countBox.Text, out var count) == false || count <= 0)
            {
                MessageBox.Show("Количество указано неверно");
                return;
            }

            var id = int.Parse(idBlock.Text);
            var product = _dbContext.Products.FirstOrDefault(x => x.Id == id);

            var boughtProduct = new BoughtProduct
            {
                Name = product.Name,
                Price = product.Price,
                ImageBase64 = product.ImageBase64,
                Size = size,
                ProductId = product.Id,
                Count = count,
                User = UserService.User
            };

            CartService.Add(boughtProduct);

            countBox.Text = "";
            MessageBox.Show("Товар успешно добавлен в корзину!");
        }
    }
}
