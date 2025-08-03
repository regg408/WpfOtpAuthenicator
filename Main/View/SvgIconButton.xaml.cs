using SkiaSharp;
using Svg.Skia;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace Main.View
{
    /// <summary>
    /// SvgIconButton.xaml 的互動邏輯
    /// </summary>
    public partial class SvgIconButton : UserControl
    {
        public SvgIconButton()
        {
            InitializeComponent();
            Loaded += SvgIconButton_Loaded;
        }

        private void SvgIconButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(SvgPath))
            {
                var svg = new SKSvg();
                using var stream = File.OpenRead(SvgPath);
                var picture = svg.Load(stream);

                var bitmap = new SKBitmap(IconSize, IconSize);
                using var surface = SKSurface.Create(new SKImageInfo(IconSize, IconSize));
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.Transparent);
                canvas.DrawPicture(picture);
                canvas.Flush();

                var image = surface.Snapshot();
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                var wpfImage = new BitmapImage();
                wpfImage.BeginInit();
                wpfImage.StreamSource = data.AsStream();
                wpfImage.CacheOption = BitmapCacheOption.OnLoad;
                wpfImage.EndInit();
                IconImage.Source = wpfImage;
            }
        }

        public string SvgPath
        {
            get => (string)GetValue(SvgPathProperty);
            set => SetValue(SvgPathProperty, value);
        }

        public static readonly DependencyProperty SvgPathProperty =
            DependencyProperty.Register(nameof(SvgPath), typeof(string), typeof(SvgIconButton), new PropertyMetadata(""));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(SvgIconButton), new PropertyMetadata(""));

        public int IconSize
        {
            get => (int)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(int), typeof(SvgIconButton), new PropertyMetadata(24));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(SvgIconButton), new PropertyMetadata(null));

        public new string ToolTip
        {
            get => (string)GetValue(ToolTipProperty);
            set => SetValue(ToolTipProperty, value);
        }

        public static new readonly DependencyProperty ToolTipProperty =
            DependencyProperty.Register(nameof(ToolTip), typeof(string), typeof(SvgIconButton), new PropertyMetadata(""));
    }
}
