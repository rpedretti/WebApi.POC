
using WebApi.Client.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WebApi.Client.Views
{
    public sealed partial class SecureChannelPage : Page
    {
        private SecureChannelViewModel ViewModel => DataContext as SecureChannelViewModel;

        public SecureChannelPage()
        {
            InitializeComponent();
        }
    }
}
