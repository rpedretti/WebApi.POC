using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebApi.Client.Shared.Models;
using WebApi.Client.Shared.Services;
using WebApi.Client.Shared.ViewModels.ParameterModels;
using WebApi.Shared;

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

        public ICommand CallUserApiCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        private MvxCommand<DemandListItem> _showDetailedDemand;
        private bool _openSecureChannel;

        public MvxCommand<DemandListItem> ShowDetailedDemand
        {
            get
            {
                return _showDetailedDemand ?? (_showDetailedDemand = new MvxCommand<DemandListItem>(demand => {
                    System.Diagnostics.Debug.WriteLine(demand.Description);
                }));
            }
        }

        public LoggedViewModel(ITestAccessService testAccessService, ILoginService loginService, IMvxNavigationService navigationService)
        {
            _testAccessService = testAccessService;
            _loginService = loginService;
            _navigationService = navigationService;
            CallUserApiCommand = new MvxCommand(CallUserApi);
            LogoutCommand = new MvxCommand(Logout);
        }

        public override void Prepare(LoggedPageParameterModel parameter)
        {
            _openSecureChannel = parameter.OpenSecureChannel;
        }

        private async void CallUserApi()
        {
            var demands = await _testAccessService.GetDemands();
            Demands.AddRange(demands.Select(d => new DemandListItem {
                Id = d.Id,
                Description = $"{d.Description} - {d.Owner.Username} ({d.Status})"
            }));            
        }

        private void Logout()
        {
            _loginService.Logout();
            _navigationService.Navigate<LoginViewModel>();
        }

        public override async Task Initialize()
        {
            if (_openSecureChannel)
            {
                await _loginService.OpenSecureChannelForLoggedUserAsync();
            }
        }
    }
}
