
using Android.App;
using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using WebApi.Client.Shared;
using WebApi.Droid.Helpers;

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
            return new App(keyStorage, storage);
        }
    }
}