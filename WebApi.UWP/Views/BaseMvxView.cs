using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.WeakSubscription;
using MvvmCross.Uwp.Views;
using System;
using WebApi.Client.Shared.Interactions;
using WebApi.Client.Shared.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WebApi.UWP.Views
{
    public abstract class BaseMvxView: MvxWindowsPage
    {
        private MvxValueEventSubscription<StatusInteraction> _serviceSubscription;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _serviceSubscription = (ViewModel as BaseViewModel).StatusMessageInteraction.WeakSubscribe(OnServiceStatus);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _serviceSubscription.DisposeIfDisposable();
        }

        protected virtual async void OnServiceStatus(object sender, MvxValueEventArgs<StatusInteraction> args)
        {
            var message = args.Value.Message;

            if (args.Value.Code != null)
            {
                message += $"for more info: code {args.Value.Code}";
            }

            var alert = new ContentDialog()
            {
                Title = args.Value.Title,
                Content = args.Value.Message,
                CloseButtonText = "OK"
            };

            await alert.ShowAsync();
        }
    }
}
