using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using WebApi.Client.Shared.Services;
using WebApi.Client.Shared.ViewModels;
using WebApi.Security;
using WebApi.Shared;

namespace WebApi.Client.Shared
{
    public sealed class App : MvxApplication
    {
        public App(IKeyStorageContainer keyStorageContainer, IStorageContainer storageContainer)
        {
            Mvx.RegisterSingleton(keyStorageContainer);
            Mvx.RegisterSingleton(storageContainer);
        }

        public override void Initialize()
        {
            base.Initialize();
            Mvx.LazyConstructAndRegisterSingleton<ITestAccessService, TestAccessService>();
            Mvx.LazyConstructAndRegisterSingleton<ISecureChannelService, SecureChannelService>();
            Mvx.LazyConstructAndRegisterSingleton<ICryptoService, CryptoService>();

            RegisterNavigationServiceAppStart<LoginViewModel>();
        }
    }
}
