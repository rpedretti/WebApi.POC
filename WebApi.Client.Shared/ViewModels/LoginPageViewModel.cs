using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WebApi.Client.Shared.Interactions;
using WebApi.Client.Shared.Services;

namespace WebApi.Client.Shared.ViewModels
{
    public sealed class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private readonly ILoginService _loginService;
        private readonly IMvxNavigationService _navigationService;
        private IMvxAsyncCommand _loginCommand;
        public IMvxAsyncCommand LoginCommand => _loginCommand ??
            (_loginCommand = new MvxAsyncCommand(
                Login,
                () =>
                {
                    return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
                }
            ));

        public string Username
        {
            get { return _username; }
            set
            {
                SetProperty(ref _username, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public LoginViewModel(ILoginService loginService, IMvxNavigationService navigationService)
        {
            _loginService = loginService;
            _navigationService = navigationService;
        }

        /// <summary>
        /// Login the user
        /// </summary>
        private async Task Login()
        {
            IsBusy = true;
            try
            {
                await _loginService.LoginAsync(Username, Password);
                await _navigationService.Navigate<LoggedViewModel>();
                if (!await _navigationService.Close(this))
                {
                    System.Diagnostics.Debug.WriteLine("error on closing");
                }
            }
            catch (UnauthorizedAccessException)
            {
                _statusMessageInteraction.Raise(new StatusInteraction
                {
                    Title = "Ops...",
                    Message = "Wrong username/password"
                });
            }
            catch (Exception e)
            {
                _statusMessageInteraction.Raise(new StatusInteraction
                {
                    Title = "Ops...",
                    Message = "Something went wrong...",
                    Code = e.HResult
                });
            }
            IsBusy = false;
        }
    }
}
