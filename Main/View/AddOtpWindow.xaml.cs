using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main.View
{
    /// <summary>
    /// AddOtpWindow.xaml 的互動邏輯
    /// </summary>
    public partial class AddOtpWindow : Window
    {
        public string ResultIssuer{ get; private set; }
        public string ResultSecret { get; private set; }
        private AddOtpWindowViewModel ViewModel => (AddOtpWindowViewModel)DataContext;

        public AddOtpWindow()
        {
            InitializeComponent();
            this.DataContext = new AddOtpWindowViewModel();
        }

        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            ResultIssuer = ViewModel.Issuer;
            ResultSecret = ViewModel.Secret;
            DialogResult = true;
            Close();
        }
    }
}
