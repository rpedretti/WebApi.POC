using CommonServiceLocator;
using WebApi.Client.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebApi.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrippleDESPage : ContentPage
    {
        public TrippleDESPage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance<TrippleDESPageViewModel>();
        }
    }
}