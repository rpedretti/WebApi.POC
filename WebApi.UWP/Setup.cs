using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;
using MvvmCross.Uwp.Platform;
using WebApi.UWP.Helpers;
using WebApi.UWP.Services;
using Windows.UI.Xaml.Controls;

namespace WebApi.UWP
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.ConstructAndRegisterSingleton<IMvxJsonConverter, JsonSerializer>();
            var preferencesManager = new PreferencesManager();
            var storageContainer = new LocalStorageContainer();
            var keyStorage = new LocalKeyStorageContainer(storageContainer);
            var deviceInformationService = new DeviceInformationService();

            return new Client.Shared.App(keyStorage, storageContainer, preferencesManager, deviceInformationService);
        }

        protected override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.None;
    }
}
