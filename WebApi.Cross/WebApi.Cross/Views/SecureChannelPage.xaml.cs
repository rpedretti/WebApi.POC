using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Cross.ViewModels;
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