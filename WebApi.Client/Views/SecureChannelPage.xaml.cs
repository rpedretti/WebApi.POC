
using WebApi.Client.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WebApi.Client.Views
{
    public sealed partial class SecureChannelPage : Page
    {
        private SecureChannelPageViewModel ViewModel => DataContext as SecureChannelPageViewModel;

        public SecureChannelPage()
        {
            InitializeComponent();
        }
    }
}
