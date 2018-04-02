using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Windows.Input;
using WebApi.Client.Shared.Interactions;
using WebApi.Client.Shared.Services;

namespace WebApi.Client.Shared.ViewModels
{
    public sealed class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private readonly IMvxNavigationService _navigationService;
        private readonly ILoginService _loginService;

        public ICommand LoginCommand { get; private set; }

        public string Username
        {
            get { return _username; }
            set
            {
                SetProperty(ref _username, value);
                (LoginCommand as MvxCommand).RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                (LoginCommand as MvxCommand).RaiseCanExecuteChanged();
            }
        }


        public LoginViewModel(            IMvxNavigationService navigationService, ILoginService loginService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
            LoginCommand = new MvxCommand(Login, () =>
            {
                return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
            });
        }

        /// <summary>
        /// Login the user
        /// </summary>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private async void Login()
        {
            IsBusy = true;
            try
            {
                await _loginService.LoginAsync(Username, Password);
                await _navigationService.Navigate<LoggedViewModel>();
            }
            catch (UnauthorizedAccessException)
            {
                _statusMessageInteraction.Raise(new StatusInteraction {
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
