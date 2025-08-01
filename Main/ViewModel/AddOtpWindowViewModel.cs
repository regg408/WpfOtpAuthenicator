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
            UploadImageCommand = new DelegateCommand();
            UploadImageCommand.ExecuteAction = new Action<object?>(this.UploadImage);
        }

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

        private string ParseOtpUriSecret(string uri)
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

        private string ParseOtpUriIssuer(string uri)
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
