using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using WebApi.Client.Shared.ViewModels;
using WebApi.Client.Shared.ViewModels.ParameterModels;
using WebApi.Shared;

namespace WebApi.Client.Shared
{
    public sealed class AppStart : IMvxAppStart
    {
        private readonly IMvxNavigationService _navigationService;

        public AppStart(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async void Start(object hint = null)
        {
            var preferencesManager = Mvx.Resolve<IPreferencesManager>();

            if (preferencesManager.Get<bool>("logged"))
            {
                await _navigationService.Navigate<LoggedViewModel, LoggedPageParameterModel>(new LoggedPageParameterModel { OpenSecureChannel = true });
            }
            else
            {
                await _navigationService.Navigate<LoginViewModel>();
            }
        }
    }
}
