using CommonServiceLocator;
using Unity;
using Unity.ServiceLocation;
using WebApi.Cross.Services;
using WebApi.Cross.ViewModels;
using WebApi.Cross.Views;
using WebApi.Security;
using WebApi.Shared;
using Xamarin.Forms;

namespace WebApi.Cross
{
    public partial class App : Application
    {

        public App(IStorageContainer storageContainer)
        {
            InitializeComponent();

            var container = new UnityContainer();

            container.RegisterInstance(storageContainer);
            container.RegisterType<ICryptoService, CryptoService>();
            container.RegisterType<ISecurityService, SecurityService>();

            container.RegisterType<RSAPageViewModel>();
            container.RegisterType<TrippleDESPageViewModel>();
            container.RegisterType<SecureChannelPageViewModel>();

            var unityServiceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
