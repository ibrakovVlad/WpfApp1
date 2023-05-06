using Market.Models;
using System.Linq;
using System.Windows;
using WpfApp1.Views;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var registerPage = new RegisterPage();
            MainFrame.Content = registerPage;
        }
    }
}
