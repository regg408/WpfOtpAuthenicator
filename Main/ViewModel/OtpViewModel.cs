using Main.Common;
using OtpNet;
using System.Net.Sockets;
using System.Windows.Threading;

namespace Main.ViewModel
{
    internal class OtpViewModel : BaseViewModel
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set { _name = value; RaisePropertyChanged(nameof(Name)); }
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

        public string Secret { get; set; }

        public DelegateCommand EditCommand { get; set; }
        public DelegateCommand EditCompletedCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        public Action? EditCompletedAction { get; set; }
        public Action<OtpViewModel>? DeleteAction { get; set; }

        private readonly Totp _totp;
        private readonly DispatcherTimer _timer;
        public OtpViewModel(string name, string issuer, string base32Secret)
        {
            Name = name;
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

            DeleteCommand = new DelegateCommand
            {
                ExecuteAction = this.Delete
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

        void Delete(object? parameter)
        {
            DeleteAction?.Invoke(this);
        }
    }
}
