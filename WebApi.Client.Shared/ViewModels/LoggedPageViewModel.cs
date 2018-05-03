using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Threading.Tasks;
using WebApi.Client.Shared.Interactions;
using WebApi.Client.Shared.Models;
using WebApi.Client.Shared.Services;
using WebApi.Client.Shared.ViewModels.ParameterModels;

namespace WebApi.Client.Shared.ViewModels
{
    public sealed class LoggedViewModel : BaseViewModel<LoggedPageParameterModel>
    {
        private ITestAccessService _testAccessService;
        private readonly ILoginService _loginService;
        private readonly IMvxNavigationService _navigationService;
        private MvxObservableCollection<DemandListItem> _demands = new MvxObservableCollection<DemandListItem>();
        public MvxObservableCollection<DemandListItem> Demands
        {
            get { return _demands; }
            set { SetProperty(ref _demands, value); }
        }

        private bool _openSecureChannel;

        public IMvxAsyncCommand _logoutCommand;
        public IMvxAsyncCommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new MvxAsyncCommand(Logout));


        public IMvxAsyncCommand _getDemandsCommand;
        public IMvxAsyncCommand GetDemandsCommand => _getDemandsCommand ??
            (_getDemandsCommand = new MvxAsyncCommand(
                GetDemandsAsync,
                () =>
                {
                    return _secureChannelOpened;
                }
            ));

        private MvxCommand<DemandListItem> _showDetailedDemand;
        public MvxCommand<DemandListItem> ShowDetailedDemand => _showDetailedDemand ??
            (_showDetailedDemand = new MvxCommand<DemandListItem>
                (demand =>
                {
                    System.Diagnostics.Debug.WriteLine(demand.Description);
                })
            );

        public bool _secureChannelOpened { get; set; }

        public LoggedViewModel(ITestAccessService testAccessService, ILoginService loginService, IMvxNavigationService navigationService)
        {
            _testAccessService = testAccessService;
            _loginService = loginService;
            _navigationService = navigationService;
        }

        public override void Prepare()
        {
            _secureChannelOpened = true;
            GetDemandsCommand.RaiseCanExecuteChanged();
        }

        public override void Prepare(LoggedPageParameterModel parameter)
        {
            _openSecureChannel = parameter.OpenSecureChannel;
        }

        public override async void ViewAppeared()
        {
            if (_openSecureChannel)
            {
                IsBusy = true;
                try
                {
                    await _loginService.OpenSecureChannelForLoggedUserAsync();
                    _secureChannelOpened = true;
                }
                catch (Exception e)
                {
                    _statusMessageInteraction.Raise(new StatusInteraction
                    {
                        Title = "Ops...",
                        Message = "Something went wrong...",
                        Code = e.HResult
                    });
                    _secureChannelOpened = false;
                }

                GetDemandsCommand.RaiseCanExecuteChanged();
                IsBusy = false;
            }
        }

        private async Task GetDemandsAsync()
        {
            IsBusy = true;
            var demands = await _testAccessService.GetDemands();
            demands.ForEach(d =>
            {
                var demand = new DemandListItem
                {
                    Id = d.Id,
                    Description = $"{d.Description} - {d.Owner.Username} ({d.Status})"
                };

                Demands.Add(demand);
            });

            IsBusy = false;
        }

        private async Task Logout()
        {
            await _loginService.LogoutAsync();
            if (!await _navigationService.Close(this))
            {
                System.Diagnostics.Debug.WriteLine("error on closing");
            }
            await _navigationService.Navigate<LoginViewModel>();
        }
    }
}
