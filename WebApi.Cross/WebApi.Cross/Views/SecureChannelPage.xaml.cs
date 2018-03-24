using CommonServiceLocator;
using WebApi.Client.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebApi.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecureChannelPage : ContentPage
    {
        public SecureChannelPage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance<SecureChannelPageViewModel>();
        }
    }
}