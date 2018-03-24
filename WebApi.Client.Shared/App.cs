using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Client.Shared.Services;
using WebApi.Client.Shared.ViewModels;
using WebApi.Security;
using WebApi.Shared;

namespace WebApi.Client.Shared
{
    public sealed class App : MvxApplication
    {
        public App(IStorageContainer storageContainer)
        {
            Mvx.RegisterSingleton(storageContainer);
        }

        public override void Initialize()
        {
            base.Initialize();
            Mvx.LazyConstructAndRegisterSingleton<ITestAccessService, TestAccessService>();
            Mvx.LazyConstructAndRegisterSingleton<ISecurityService, SecurityService>();
            Mvx.LazyConstructAndRegisterSingleton<ICryptoService, CryptoService>();

            RegisterNavigationServiceAppStart<LoginViewModel>();
        }
    }
}
