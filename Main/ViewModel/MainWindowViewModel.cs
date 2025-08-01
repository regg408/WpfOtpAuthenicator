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
            AddOtpCommand = new DelegateCommand();
            AddOtpCommand.ExecuteAction = new Action<object?>(this.AddOtp);
        }

        void AddOtp(object? parameter)
        {
            var win = new AddOtpWindow();
            if (win.ShowDialog() == true && !string.IsNullOrWhiteSpace(win.ResultSecret))
            {
                OtpItems.Add(new OtpViewModel(win.ResultIssuer, win.ResultSecret));
            }
        }
    }
}
