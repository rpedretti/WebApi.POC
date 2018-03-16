using System;

using WebApi.Client.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WebApi.Client.Views
{
    public sealed partial class TrippleDESPage : Page
    {
        private TrippleDESViewModel ViewModel => DataContext as TrippleDESViewModel;

        public TrippleDESPage()
        {
            InitializeComponent();
        }
    }
}
