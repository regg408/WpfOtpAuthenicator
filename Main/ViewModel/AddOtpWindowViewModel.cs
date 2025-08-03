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
                    var parsed = ParseOtpUriSecret(result.Text);
                    if (!string.IsNullOrWhiteSpace(parsed))
                    {
                        Issuer = ParseOtpUriIssuer(result.Text);
                        Secret = parsed;
                        CanConfirm = true;
                    }
                }
            }
        }

        /// <summary>
        /// 解析OTP uri密鑰
        /// </summary>
        /// <param name="uri">OTP uri</param>
        /// <returns>OTP密鑰</returns>
        private static string ParseOtpUriSecret(string uri)
        {
            try
            {
                var parsed = new Uri(uri);
                var query = HttpUtility.ParseQueryString(parsed.Query);
                return query["secret"] ?? "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解析OTP uri發行來源
        /// </summary>
        /// <param name="uri">OTP uri</param>
        /// <returns>OTP發行來源</returns>
        private static string ParseOtpUriIssuer(string uri)
        {
            try
            {
                var parsed = new Uri(uri);
                var query = HttpUtility.ParseQueryString(parsed.Query);
                return query["issuer"] ?? "";
            }
            catch
            {
                return "";
            }
        }
    }
}
