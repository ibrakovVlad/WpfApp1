using Market.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Market.Views
{
    public partial class EditProductWindow : Window
    {
        private readonly static Uri _defaultUserPhotoUri = new Uri("/Images/Register.jpg", UriKind.Relative);

        private readonly Product _product;
        private readonly MyDbContext _dbContext;

        public EditProductWindow(Product product)
        {
            InitializeComponent();
            _product = product;
            _dbContext = MyDbContext.GetContext();

            BoxName.Text = _product.Name;
            BoxPrice.Text = _product.Price.ToString();
            
            if (_product.ImageBase64 == null || _product.ImageBase64.Length < 1)
            {
                SetPhotoUri(_defaultUserPhotoUri);
            }
            else
            {
                try
                {
                    ProductImage.Source = Base64ToImage(_product.ImageBase64);
                }
                catch
                {
                    SetPhotoUri(_defaultUserPhotoUri);
                }
            }
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
            _product.ImageBase64 = base64Photo;

            _product.Name = name;
            _product.Price = price;
            _dbContext.SaveChanges();
            DialogResult = true;
        }

        private void SetPhotoUri(Uri uri)
        {
            ProductImage.Source = new BitmapImage(uri);
        }

        public BitmapSource Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream stream = new MemoryStream(imageBytes, 0, imageBytes.Length)
            {
                Position = 0
            };
            BitmapImage result = new BitmapImage();
            result.BeginInit();
            result.CacheOption = BitmapCacheOption.OnLoad;
            result.StreamSource = stream;
            result.EndInit();
            result.Freeze();
            return result;
        }
    }
}
