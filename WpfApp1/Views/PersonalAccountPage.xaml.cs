using Market.Models;
using Market.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using WpfApp1.Views;

namespace Market.Views
{
    public partial class PersonalAccountPage : Page
    {
        private readonly MyDbContext _dbContext;
        private readonly User _user;
     
        public PersonalAccountPage(User user)
        {
            InitializeComponent();
            _dbContext = MyDbContext.GetContext();
            _user = user;
            BoxLogin.Text = user.Login;
            BoxPass.Password = user.Password;
            BoxPass2.Password = user.Password;
            BoxEmail.Text = user.Email;
        }

        private void Button_reg(object sender, RoutedEventArgs e)
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
                    var user = _user;

                    var emailsUsers = _dbContext.Users.Where(x => x.Email == BoxEmail.Text)
                        .ToList();

                    foreach (var emailUser in emailsUsers)
                    {
                        if (emailUser != user)
                        {
                            MessageBox.Show("Почта занята");
                            return;
                        }
                    }

                    var loginsUsers = _dbContext.Users.Where(x => x.Login == BoxLogin.Text)
                        .ToList();

                    foreach (var loginUser in loginsUsers)
                    {
                        if (loginUser != user)
                        {
                            MessageBox.Show("Логин занят");
                            return;
                        }
                    }

                    user.Email = BoxEmail.Text;
                    user.Login = BoxLogin.Text;
                    user.Password = BoxPass.Password;
                    _dbContext.SaveChanges();
                    MessageBox.Show("Редактирование прошло успешно!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка редактирования профиля: " + ex.Message);
                    return;
                }
            }
        }

        private void Button_del(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = _user;
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("При удалении произошла ошибка: " + ex.Message);
                return;
            }

            if (_user == UserService.User)
            {
                UserService.User = null;
                NavigationService.Navigate(new RegisterPage());
            }
            else
            {
                NavigationService.Navigate(new UsersPage());
            }
        }

        private void DoGoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
