using MvvmCross.Core.Navigation;
using WebApi.Client.Shared.Services;
using WebApi.Client.Shared.ViewModels.ParameterModels;

namespace WebApi.Client.Shared.ViewModels
{
    public sealed class LaucherViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly ILoginService _loginService;

        public LaucherViewModel(IMvxNavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
        }

        public override async void ViewAppeared()
        {
            if (_loginService.IsUserLogged())
            {
                await _navigationService.Navigate<LoggedViewModel, LoggedPageParameterModel>(new LoggedPageParameterModel { OpenSecureChannel = true });
            }
            else
            {
                await _navigationService.Navigate<LoginViewModel>();
            }

            await _navigationService.Close(this);
        }
    }
}
