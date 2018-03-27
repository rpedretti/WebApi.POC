using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WebApi.Client.Shared.Interactions;
using WebApi.Client.Shared.Services;
using WebApi.Shared.Domain;

namespace WebApi.Client.Shared.ViewModels
{
    public sealed class LoggedViewModel : BaseViewModel
    {
        private ITestAccessService _testAccessService;
        private MvxObservableCollection<ServiceDemand> _demands = new MvxObservableCollection<ServiceDemand>();
        public MvxObservableCollection<ServiceDemand> Demands
        {
            get { return _demands; }
            set { SetProperty(ref _demands, value); }
        }

        public ICommand CallUserApiCommand { get; private set; }

        private MvxCommand<ServiceDemand> _showDetailedDemand;
        public MvxCommand<ServiceDemand> ShowDetailedDemand
        {
            get
            {
                return _showDetailedDemand ?? (_showDetailedDemand = new MvxCommand<ServiceDemand>(demand => {
                    System.Diagnostics.Debug.WriteLine($"{demand.Description} ({demand.Id})");
                }));
            }
        }

        public LoggedViewModel(ITestAccessService testAccessService)
        {
            _testAccessService = testAccessService;
            CallUserApiCommand = new MvxCommand(CallUserApi);
        }

        private async void CallUserApi()
        {
            var demands = await _testAccessService.GetDemands();
            Demands.AddRange(demands);            
        }
    }
}
