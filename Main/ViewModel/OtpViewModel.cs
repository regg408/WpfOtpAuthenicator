using Main.Common;
using Main.View;
using OtpNet;
using System.Windows;
using System.Windows.Threading;

namespace Main.ViewModel
{
    internal class OtpViewModel : BaseViewModel
    {
        public string QrCodeUri { get; set; } = "";

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
            private set { _issuer = value; RaisePropertyChanged(nameof(Issuer)); }
        }

        private string _otpCode = "";
        public string OtpCode
        {
            get => _otpCode;
            set { _otpCode = value; RaisePropertyChanged(nameof(OtpCode)); }
        }

        private string _timeLeft = "";
        public string TimeLeft
        {
            get => _timeLeft;
            set { _timeLeft = value; RaisePropertyChanged(nameof(TimeLeft)); }
        }

        private bool _isEdit = false;
        public bool IsEdit
        {
            get => _isEdit;
            set { _isEdit = value; RaisePropertyChanged(nameof(IsEdit)); }
        }

        private string _copiedText = "";
        public string CopiedText
        {
            get => _copiedText;
            set { _copiedText = value; RaisePropertyChanged(nameof(CopiedText)); }
        }

        public string Secret { get; set; }

        public DelegateCommand EditCommand { get; set; }
        public DelegateCommand EditCompletedCommand { get; set; }
        public DelegateCommand ShowQrCodeCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand CopyCommand { get; set; }

        public Action? EditCompletedAction { get; set; }
        public Action<OtpViewModel>? DeleteAction { get; set; }

        private readonly Totp _totp;
        private readonly DispatcherTimer _timer;
        public OtpViewModel(string uri, string name, string issuer, string base32Secret)
        {
            QrCodeUri = uri;
            Label = name;
            Issuer = issuer;
            Secret = base32Secret;
            EditCommand = new DelegateCommand
            {
                ExecuteAction = this.Edit
            };

            EditCompletedCommand = new DelegateCommand
            {
                ExecuteAction = this.CompletedEdit
            };

            ShowQrCodeCommand = new DelegateCommand
            {
                ExecuteAction = this.ShowQrCode
            };

            DeleteCommand = new DelegateCommand
            {
                ExecuteAction = this.Delete
            };

            CopyCommand = new DelegateCommand
            {
                ExecuteAction = this.Copy
            };

            var secret = Base32Encoding.ToBytes(base32Secret);
            _totp = new Totp(secret);
            UpdateOtp();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (_, __) => UpdateOtp();
            _timer.Start();
        }

        private void UpdateOtp()
        {
            OtpCode = _totp.ComputeTotp();
            TimeLeft = $"{_totp.RemainingSeconds()}s";
        }

        void Edit(object? parameter)
        {
            IsEdit = true;
        }

        void CompletedEdit(object? parameter)
        {
            IsEdit = false;
            EditCompletedAction?.Invoke();
        }

        void ShowQrCode(object? parameter)
        {
            var win = new QrCodeWindow(QrCodeUri);
            win.ShowDialog();
        }

        void Delete(object? parameter)
        {
            DeleteAction?.Invoke(this);
        }

        async void Copy(object? parameter)
        {
            Clipboard.SetText(OtpCode);
            CopiedText = "✔ Copied";
            await Task.Delay(1500); // 顯示 1.5 秒
            CopiedText = string.Empty;
        }
    }
}
