﻿using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using WebApi.Client.Shared.ViewModels;
using WebApi.Shared;

namespace WebApi.Client.Shared
{
    public sealed class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            var preferencesManager = Mvx.Resolve<IPreferencesManager>();

            if (preferencesManager.Get<bool>("logged"))
            {
                ShowViewModel<LoggedViewModel>();
            }
            else
            {
                ShowViewModel<LoginViewModel>();
            }
        }
    }
}
