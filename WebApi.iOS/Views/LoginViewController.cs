using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform.Core;
using UIKit;
using WebApi.Client.Shared.Interactions;
using WebApi.Client.Shared.ViewModels;

namespace WebApi.iOS.Views
{
    public partial class LoginViewController : MvxViewController<LoginViewModel>
    {
        public LoginViewController() : base("LoginViewController", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.StatusMessageInteraction.WeakSubscribe(OnServiceStatus);

            var set = this.CreateBindingSet<LoginViewController, LoginViewModel>();
            set.Bind(Username).To(vm => vm.Username).OneWayToSource();
            set.Bind(Password).To(vm => vm.Password).OneWayToSource();
            set.Bind(LoginButton).To(vm => vm.LoginCommand);

            set.Apply();
        }

        private void OnServiceStatus(object sender, MvxValueEventArgs<StatusInteraction> args)
        {
            UIAlertView alert = new UIAlertView()
            {
                Title = args.Value.Title,
                Message = args.Value.Message
            };
            alert.AddButton("OK");
            alert.Show();
        }
    }
}