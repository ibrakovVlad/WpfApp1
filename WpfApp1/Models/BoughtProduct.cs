using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Media.Imaging;

namespace Market.Models
{
    public class BoughtProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        [MaxLength]
        public string ImageBase64 { get; set; }
        public string Size { get; set; }
        public int Count { get; set; }
        public User User { get; set; }

        public BitmapSource Image => GetImage();
        public string PriceView => Price.ToString() + '₽';
        public string SizeView => "Размер: " + Size.ToString();
        public string CountView => "Количество: " + Count.ToString();

        private BitmapSource GetImage()
        {
            byte[] imageBytes = Convert.FromBase64String(ImageBase64);
            MemoryStream stream = new MemoryStream(imageBytes, 0, imageBytes.Length);
            stream.Position = 0;
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
