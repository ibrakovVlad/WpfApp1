using Market.Models;
using Market.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1;
using WpfApp1.Views;

namespace Market.Views
{
    public partial class LoginPage : Page
    {
        private readonly MyDbContext _dbContext;

        public LoginPage()
        {
            InitializeComponent();
            _dbContext = MyDbContext.GetContext();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var registerPage = new RegisterPage();
            NavigationService.Navigate(registerPage);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string login = BoxLogin.Text.Trim();
            string pass = BoxPass.Password.Trim();

            if (login.Length < 5)
            {

                BoxLogin.ToolTip = "Не корректно!";
                BoxLogin.Background = Brushes.Blue;
            }
            else if (pass.Length < 5)
            {

                BoxPass.ToolTip = "Не корректно!";
                BoxPass.Background = Brushes.Blue;
            }
            else
            {
                BoxLogin.ToolTip = "";
                BoxLogin.Background = Brushes.Transparent;
                BoxPass.ToolTip = "";
                BoxPass.Background = Brushes.Transparent;

                try
                {
                    var user = _dbContext.Users.FirstOrDefault(x => x.Login == BoxLogin.Text && x.Password == BoxPass.Password)
                        ?? throw new Exception("Пользователь не зарегистрирован");
                    UserService.User = user;
                    var buyProductsPage = new BuyProductsPage();
                    NavigationService.Navigate(buyProductsPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Авторизация прошла неудачно: " + ex.Message);
                }
            }
        }
    }
}
