using Android.App;
using Android.OS;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform.Core;
using WebApi.Client.Shared.Interactions;
using WebApi.Client.Shared.ViewModels;

namespace WebApi.Droid.Views
{
    [Activity(Label = "Login", NoHistory = true)]
    public class LoginActivity : MvxActivity<LoginViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            ViewModel.StatusMessageInteraction.WeakSubscribe(OnServiceStatus);
        }
        
        private void OnServiceStatus(object sender, MvxValueEventArgs<StatusInteraction> args)
        {
            RunOnUiThread(() =>
            {
                var message = args.Value.Message;

                if (args.Value.Code != null)
                {
                    message += $"for more info: code {args.Value.Code}";
                }

                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle(args.Value.Title);
                alert.SetMessage(args.Value.Message);
                alert.SetNeutralButton("OK", (c, e) => { });
                Dialog dialog = alert.Create();
                dialog.Show();
            });
        }
    }
}

