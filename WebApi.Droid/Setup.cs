using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;
using Unity;
using WebApi.Client.Shared;
using WebApi.Client.Shared.Services;
using WebApi.Client.Shared.ViewModels;
using WebApi.Droid.Helpers;
using WebApi.Security;

namespace WebApi.Droid
{
    [Application]
    public sealed class Setup : MvxAndroidSetup
    {
        public Setup(Context context) : base(context) { }

        protected override IMvxApplication CreateApp()
        {
            return new App(new LocalStorageContainer());
        }
    }
}