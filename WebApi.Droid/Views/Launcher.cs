using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using WebApi.Client.Shared.ViewModels;

namespace WebApi.Droid.Views
{
    [Activity(Label = "WebApi POC", MainLauncher = true, NoHistory = true, Theme = "@style/AppTheme.SplashTheme")]
    public class Launcher : MvxActivity<LaucherViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}