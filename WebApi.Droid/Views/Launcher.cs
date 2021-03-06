﻿using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using WebApi.Client.Shared.ViewModels;

namespace WebApi.Droid.Views
{
    [Activity(Label = "WebApi POC", MainLauncher = true, Theme = "@style/AppTheme.SplashTheme")]
    public class Launcher : MvxAppCompatActivity<LaucherViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}