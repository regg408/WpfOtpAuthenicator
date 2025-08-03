using Main.ViewModel;
using System.Windows;

namespace Main.View
{
    /// <summary>
    /// AddOtpWindow.xaml 的互動邏輯
    /// </summary>
    public partial class AddOtpWindow : Window
    {
        public string ResultUri { get; private set; } = "";
        public string ResultLabel { get; private set; } = "";
        public string ResultIssuer { get; private set; } = "";
        public string ResultSecret { get; private set; } = "";
        private AddOtpWindowViewModel ViewModel => (AddOtpWindowViewModel)DataContext;

        public AddOtpWindow()
        {
            InitializeComponent();
            this.DataContext = new AddOtpWindowViewModel();
        }

        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            ResultUri = ViewModel.OtpUri;
            ResultLabel = ViewModel.Label;
            ResultIssuer = ViewModel.Issuer;
            ResultSecret = ViewModel.Secret;
            DialogResult = true;
            Close();
        }
    }
}
