using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WebApi.Client.Shared.Interactions;
using WebApi.Client.Shared.Services;

namespace WebApi.Client.Shared.ViewModels
{
    public sealed class LoggedViewModel : BaseViewModel
    {
        private ITestAccessService _testAccessService;

        public ICommand CallUserApiCommand { get; private set; }
        public ICommand CallAdminApiCommand { get; private set; }

        public LoggedViewModel(ITestAccessService testAccessService)
        {
            _testAccessService = testAccessService;
            CallUserApiCommand = new MvxCommand(CallUserApi);
            CallAdminApiCommand = new MvxCommand(CallAdminApi);
        }

        private async void CallUserApi()
        {
            var result = new StatusInteraction
            {
                Title = "Access Status"
            };

            if (await _testAccessService.CallUserApiAsync())
            {
                result.Code = 200;
                result.Message = "You have access";
            }
            else
            {
                result.Code = 401;
                result.Message = "You don't have access";
            }

            StatusMessageInteraction.Raise(result);
        }

        private async void CallAdminApi()
        {

            var result = new StatusInteraction
            {
                Title = "Access Status"
            };

            if (await _testAccessService.CallAdminApiAsync())
            {
                result.Code = 200;
                result.Message = "You have access";
            }
            else
            {
                result.Code = 401;
                result.Message = "You don't have access";
            }

            StatusMessageInteraction.Raise(result);
        }
    }
}
