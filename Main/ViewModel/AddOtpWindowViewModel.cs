using Main.Common;
using Microsoft.Win32;
using System.Drawing;
using System.Web;
using ZXing.Windows.Compatibility;

namespace Main.ViewModel
{
    internal class AddOtpWindowViewModel : BaseViewModel
    {
        public DelegateCommand UploadImageCommand { get; set; }

        private bool _canConfirm;
        public bool CanConfirm
        {
            get => _canConfirm;
            set { _canConfirm = value; RaisePropertyChanged(nameof(CanConfirm)); }
        }

        public string OtpUri { get; set; } = "";

        private string _label = "";
        public string Label
        {
            get => _label;
            set { _label = value; RaisePropertyChanged(nameof(Label)); }
        }

        private string _issuer = "";
        public string Issuer
        {
            get => _issuer;
            set { _issuer = value; RaisePropertyChanged(nameof(Issuer)); }
        }

        private string _secret = "";
        public string Secret
        {
            get => _secret;
            set { _secret = value; RaisePropertyChanged(nameof(Secret)); }
        }

        public AddOtpWindowViewModel()
        {
            UploadImageCommand = new DelegateCommand
            {
                ExecuteAction = new Action<object?>(this.UploadImage)
            };
        }

        /// <summary>
        /// 上傳QR Code圖片
        /// </summary>
        /// <param name="parameter"></param>
        void UploadImage(object? parameter)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                var bitmap = new Bitmap(dlg.FileName);
                var result = new BarcodeReader().Decode(bitmap);
                if (result != null)
                {
                    var uri = new Uri(result.Text);
                    var otpProperty = ParseOtpUri(new Uri(result.Text));
                    if (!string.IsNullOrWhiteSpace(otpProperty.Secret))
                    {
                        OtpUri = uri.ToString();
                        Label = otpProperty.Label;
                        Issuer = otpProperty.Issuer;
                        Secret = otpProperty.Secret;
                        CanConfirm = true;
                    }
                }
            }
        }

        static OtpProperty ParseOtpUri(Uri uri)
        {
            var type = uri.Host;
            var label = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/'));
            var parameters = HttpUtility.ParseQueryString(uri.Query);

            return new OtpProperty
            {
                Type = type,
                Label = label,
                Secret = parameters["secret"] ?? "",
                Issuer = parameters["issuer"] ?? ""
            };
        }
    }
}
