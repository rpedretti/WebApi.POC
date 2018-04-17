using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Attributes;
using WebApi.Client.Shared.ViewModels;

namespace WebApi.UWP.Views
{
    [MvxViewFor(typeof(LoginViewModel))]
    [MvxPagePresentation]
    public sealed partial class LoginPage : BaseMvxView
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public LoginViewModel VM => ViewModel as LoginViewModel;
    }
}
