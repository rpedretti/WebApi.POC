using CommonServiceLocator;
using Unity;
using Unity.ServiceLocation;
using WebApi.Client.Shared.Services;
using WebApi.Client.Shared.ViewModels;
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

            RegisterServices(storageContainer, container);
            RegisterViewModels(container);

            var unityServiceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);

            MainPage = new NavigationPage(new LoginPage());
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

        private static void RegisterServices(IStorageContainer storageContainer, UnityContainer container)
        {
            container.RegisterInstance(storageContainer);
            container.RegisterType<ICryptoService, CryptoService>();
            container.RegisterType<ISecurityService, SecurityService>();
        }

        private static void RegisterViewModels(UnityContainer container)
        {
            container.RegisterType<RSAPageViewModel>();
            container.RegisterType<TrippleDESPageViewModel>();
            container.RegisterType<SecureChannelPageViewModel>();
            container.RegisterType<LoginPageViewModel>();
        }
    }
}
