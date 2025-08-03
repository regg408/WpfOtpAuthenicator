using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZXing;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace Main.View
{
    /// <summary>
    /// QrCodeWindow.xaml 的互動邏輯
    /// </summary>
    public partial class QrCodeWindow : Window
    {
        string QrCodeUri { get; set; }

        public QrCodeWindow(string qrCodeUri)
        {
            QrCodeUri = qrCodeUri;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 350,
                    Width = 350,
                }
            };

            var memory = new MemoryStream();
            writer.Write(QrCodeUri).Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            this.QrCodeImage.Source = bitmapImage;
        }
    }
}
