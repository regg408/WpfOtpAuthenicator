using Main.Common;
using Main.View;
using System.Collections.ObjectModel;

namespace Main.ViewModel
{
    internal class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<OtpViewModel> OtpItems { get; set; } = [];
        public DelegateCommand AddOtpCommand { get; set; }

        public MainWindowViewModel()
        {
            AddOtpCommand = new DelegateCommand
            {
                ExecuteAction = this.AddOtp
            };
            this.Load();
        }

        /// <summary>
        /// 新增OTP
        /// </summary>
        /// <param name="parameter"></param>
        void AddOtp(object? parameter)
        {
            var win = new AddOtpWindow();
            if (win.ShowDialog() == true && !string.IsNullOrWhiteSpace(win.ResultSecret))
            {
                var item = new OtpViewModel(win.ResultUri, win.ResultLabel, win.ResultIssuer, win.ResultSecret)
                {
                    EditCompletedAction = this.Save,
                    DeleteAction = this.Delete
                };
                OtpItems.Add(item);
                this.Save();
            }
        }

        /// <summary>
        /// 儲存
        /// </summary>
        void Save()
        {
            List<OtpAccount> otps = [];
            foreach (var item in OtpItems)
            {
                otps.Add(new OtpAccount
                {
                    Uri = item.QrCodeUri,
                    Label = item.Label,
                    Issuer = item.Issuer,
                    Secret = item.Secret
                });
            }
            StorageService.Save(otps);
        }

        void Load()
        {
            var otps = StorageService.Load();
            foreach (var otp in otps)
            {
                var item = new OtpViewModel(otp.Uri, otp.Label, otp.Issuer, otp.Secret)
                {
                    EditCompletedAction = this.Save,
                    DeleteAction = this.Delete
                };
                OtpItems.Add(item);
            }
        }

        void Delete(OtpViewModel otp)
        {
            OtpItems.Remove(otp);
            Save();
        }
    }
}
