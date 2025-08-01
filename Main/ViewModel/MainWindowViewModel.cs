using Main.Common;
using Main.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        void AddOtp(object? parameter)
        {
            var win = new AddOtpWindow();
            if (win.ShowDialog() == true && !string.IsNullOrWhiteSpace(win.ResultSecret))
            {
                var item = new OtpViewModel("unknown", win.ResultIssuer, win.ResultSecret)
                {
                    EditCompletedAction = this.Save,
                    DeleteAction = this.Delete
                };
                OtpItems.Add(item);
            }
        }

        void Save()
        {
            List<OtpAccount> ptps = [];
            foreach(var item in OtpItems)
            {
                ptps.Add(new OtpAccount
                {
                    Name = item.Name,
                    Issuer = item.Issuer,
                    Secret = item.Secret
                });
            }
            StorageService.Save(ptps);
        }

        void Load()
        {
            var otps = StorageService.Load();
            foreach(var otp in otps)
            {
                var item = new OtpViewModel(otp.Name, otp.Issuer, otp.Secret)
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
