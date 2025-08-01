using Main.Common;
using OtpNet;
using System.Windows.Threading;

namespace Main.ViewModel
{
    internal class OtpViewModel : BaseViewModel
    {
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


        private readonly Totp _totp;
        private readonly DispatcherTimer _timer;
        public OtpViewModel(string issuer, string base32Secret)
        {
            Issuer = issuer;

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
            TimeLeft = $"Expires in: {_totp.RemainingSeconds()}s";
        }
    }
}
