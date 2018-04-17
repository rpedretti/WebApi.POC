using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform.Core;
using UIKit;
using WebApi.Client.Shared.Interactions;
using WebApi.Client.Shared.ViewModels;

namespace WebApi.iOS.Views
{
    public partial class LoggedViewController : MvxViewController<LoggedViewModel>
    {
        public LoggedViewController() : base("LoggedViewController", null)
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
            var source = new MvxSimpleTableViewSource(DemandList, "DemandCell", DemandCell.Key);
            DemandList.RowHeight = 60;

            var set = this.CreateBindingSet<LoggedViewController, LoggedViewModel>();
            set.Bind(FetchButton).To((LoggedViewModel vm) => vm.GetDemandsCommand);
            set.Bind(source).To(vm => vm.Demands);

            
            set.Apply();

            DemandList.Source = source;
            DemandList.ReloadData();
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