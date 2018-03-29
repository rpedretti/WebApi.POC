﻿using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using WebApi.Client.Shared.ViewModels;

namespace WebApi.Client.Shared
{
    public sealed class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public AppStart(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;
        }

        public void Start(object hint = null)
        {
            ShowViewModel<LoginViewModel>();
        }
    }
}
