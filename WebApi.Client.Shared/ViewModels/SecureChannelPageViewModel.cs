using MvvmCross.Core.ViewModels;
using System;
using System.Windows.Input;
using WebApi.Client.Shared.Services;

namespace WebApi.Client.Shared.ViewModels
{
    public class SecureChannelPageViewModel : BaseViewModel
    {
        private ISecurityService _securityService;
        private string _message;
        private string _username;
        private string _password;
        private string _response;

        public ICommand RequestSecureChannelCommand { get; private set; }
        public ICommand SendSecureMessageCommand { get; private set; }

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string Response
        {
            get { return _response; }
            set { SetProperty(ref _response, value); }
        }


        public SecureChannelPageViewModel(ISecurityService securityService)
        {
            _securityService = securityService;
            RequestSecureChannelCommand = new MvxCommand(RequestSecureChannel);
            SendSecureMessageCommand = new MvxCommand(SendSecureMessage);
        }

        public async void RequestSecureChannel()
        {
            try
            {
                await _securityService.OpenSecureChannelAsync(Username, Password);
                Response = "Success";
            }
            catch (Exception e)
            {
                Response = e.Message;
            }
        }

        /// <summary>
        /// Sends a message over a secure channel
        /// </summary>
        public async void SendSecureMessage()
        {
            Response = await _securityService.SendMessageOnSecureChannel(Message);
        }
    }
}
