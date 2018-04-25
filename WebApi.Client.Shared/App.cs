using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using WebApi.Client.Shared.Services;
using WebApi.Security;
using WebApi.Shared;

namespace WebApi.Client.Shared
{
    public sealed class App : MvxApplication
    {
        public App(
            IKeyStorageContainer keyStorageContainer, 
            IStorageContainer storageContainer,
            IPreferencesManager preferencesManager,
            IDeviceInformationService deviceInformationService)
        {
            Mvx.RegisterSingleton(keyStorageContainer);
            Mvx.RegisterSingleton(storageContainer);
            Mvx.RegisterSingleton(preferencesManager);
            Mvx.RegisterSingleton(deviceInformationService);
        }

        public override void Initialize()
        {
            base.Initialize();
            Mvx.LazyConstructAndRegisterSingleton<ITestAccessService, TestAccessService>();
            Mvx.LazyConstructAndRegisterSingleton<ISecureChannelService, SecureChannelService>();
            Mvx.LazyConstructAndRegisterSingleton<ICryptoService, CryptoService>();
            Mvx.LazyConstructAndRegisterSingleton<ILoginService, LoginService>();

            RegisterCustomAppStart<AppStart>();
        }
    }
}
