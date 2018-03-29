using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using WebApi.Client.Shared;
using WebApi.iOS.Helpers;

namespace WebApi.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(MvxApplicationDelegate appDelegate, IMvxIosViewPresenter presenter)
    : base(appDelegate, presenter)
        {
        }


        protected override IMvxApplication CreateApp()
        {
            var storage = new LocalStorageContainer();
            var keyStorage = new LocalKeyStorageContainer(storage);
            return new App(keyStorage, storage);
        }
    }
}