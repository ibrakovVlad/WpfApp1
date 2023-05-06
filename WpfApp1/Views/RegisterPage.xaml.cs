using System.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Market.Views;
using Market.Models;
using Market.Services;
using System.Linq;

namespace WpfApp1.Views
{
    public partial class RegisterPage : Page
    {
        private readonly MyDbContext _dbContext;

        public RegisterPage()
        {
            InitializeComponent();
            _dbContext = MyDbContext.GetContext();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var loginPage = new LoginPage();
            NavigationService.Navigate(loginPage);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string login = BoxLogin.Text.Trim();
            string pass = BoxPass.Password.Trim();
            string pass2 = BoxPass2.Password.Trim();
            string email = BoxEmail.Text.Trim().ToLower();

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
            else if (pass != pass2)
            {

                BoxPass2.ToolTip = "Не корректно!";
                BoxPass2.Background = Brushes.Blue;
            }
            else if (email.Length < 5 || !email.Contains('@') || !email.Contains('.'))
            {

                BoxEmail.ToolTip = "Не корректно!";
                BoxEmail.Background = Brushes.Blue;
            }
            else
            {
                BoxLogin.ToolTip = "";
                BoxLogin.Background = Brushes.Transparent;
                BoxPass.ToolTip = "";
                BoxPass.Background = Brushes.Transparent;
                BoxPass2.ToolTip = "";
                BoxPass2.Background = Brushes.Transparent;
                BoxEmail.ToolTip = "";
                BoxEmail.Background = Brushes.Transparent;

                try
                {
                    var existedUser = _dbContext.Users.FirstOrDefault(x => x.Login == BoxLogin.Text || x.Email == BoxEmail.Text);
                    if (existedUser != null)
                        throw new Exception("Пользователь с такой почтой или логином уже сущесвует");

                    var user = new User
                    {
                        Email = BoxEmail.Text,
                        Login = BoxLogin.Text,
                        Password = BoxPass.Password
                    };
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                    UserService.User = user;
                    MessageBox.Show("Регистрация успешна!");
                    var buyProductsPage = new BuyProductsPage();
                    NavigationService.Navigate(buyProductsPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка регистрации: " + ex.Message);
                    return;
                }
            }
        }
    }
}
