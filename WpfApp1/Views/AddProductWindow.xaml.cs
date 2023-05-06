using Market.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Market.Views
{
    public partial class AddProductWindow : Window
    {
        private readonly static Uri _defaultUserPhotoUri = new Uri("/Images/Register.jpg", UriKind.Relative);
        
        private readonly MyDbContext _dbContext;
        
        private Uri _userPhotoCurrentUri;

        public AddProductWindow()
        {
            InitializeComponent();
            _dbContext = MyDbContext.GetContext();
            SetPhotoUri(_defaultUserPhotoUri);
        }

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image | *.jpg;*.jpeg;*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                SetPhotoUri(new Uri(openFileDialog.FileName));
            }
        }

        private void SaveProduct(object sender, RoutedEventArgs e)
        {
            var name = BoxName.Text;
            if (name == null || name.Length < 1)
            {
                MessageBox.Show("Имя указано неверно");
                return;
            }

            if (int.TryParse(BoxPrice.Text, out var price) == false)
            {
                MessageBox.Show("Цена указана неверно");
                return;
            }

            if (_userPhotoCurrentUri == null)
            {
                MessageBox.Show("Изображение указано неверно");
                return;
            }

            var image = (BitmapImage)ProductImage.Source;

            byte[] photoBytes;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                photoBytes = ms.ToArray();
            }

            string base64Photo = Convert.ToBase64String(photoBytes);

            var product = new Product
            {
                Name = name,
                Price = price,
                ImageBase64 = base64Photo
            };

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            DialogResult = true;
        }

        private void SetPhotoUri(Uri uri)
        {
            _userPhotoCurrentUri = uri;
            ProductImage.Source = new BitmapImage(uri);
        }
    }
}
