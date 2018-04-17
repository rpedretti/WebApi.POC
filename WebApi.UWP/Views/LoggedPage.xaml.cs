using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Attributes;
using WebApi.Client.Shared.ViewModels;

namespace WebApi.UWP.Views
{
    [MvxViewFor(typeof(LoggedViewModel))]
    [MvxPagePresentation]
    public sealed partial class LoggedPage : BaseMvxView
    {
        public LoggedPage()
        {
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
            InitializeComponent();
        }

        public LoggedViewModel VM => ViewModel as LoggedViewModel;
    }
}
