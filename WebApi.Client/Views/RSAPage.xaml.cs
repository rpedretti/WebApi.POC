using System;

using WebApi.Client.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WebApi.Client.Views
{
    public sealed partial class RSAPage : Page
    {
        private RSAViewModel ViewModel => DataContext as RSAViewModel;

        public RSAPage()
        {
            InitializeComponent();
        }
    }
}
