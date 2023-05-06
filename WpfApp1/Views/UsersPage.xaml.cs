using Market.Models;
using Market.Views;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfApp1.Views
{
    public partial class UsersPage : Page
    {
        private User _selected;
        private readonly MyDbContext _dbContext;

        public UsersPage()
        {
            InitializeComponent();
            _dbContext = MyDbContext.GetContext();
            UpdateList();
        }

        private void DoGoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BuyProductsPage());
        }
        
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selected = (User)UsersList.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == null)
            {
                MessageBox.Show("Пользователь не выбран");
                return;
            }    

            var personalAccountPage = new PersonalAccountPage(_selected);
            NavigationService.Navigate(personalAccountPage);
        }

        private void UpdateList()
        {
            UsersList.ItemsSource = _dbContext.Users.ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateList();
        }
    }
}
