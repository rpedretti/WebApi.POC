using Android.App;
using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using WebApi.Client.Shared;
using WebApi.Droid.Helpers;
using WebApi.Droid.Services;

namespace WebApi.Droid
{
    [Application]
    public sealed class Setup : MvxAndroidSetup
    {
        public Setup(Context context) : base(context) { }

        protected override IMvxApplication CreateApp()
        {
            var storage = new LocalStorageContainer();
            var keyStorage = new LocalKeyStorageContainer(storage);
            var preferencesManager = new PreferencesManager();
            var deviceInformationService = new DeviceInformationService();

            return new App(keyStorage, storage, preferencesManager, deviceInformationService);
        }
    }
}