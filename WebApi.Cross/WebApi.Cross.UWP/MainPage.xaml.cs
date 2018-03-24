using WebApi.Cross.UWP.Helpers;

namespace WebApi.Cross.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new WebApi.Cross.App(new LocalStorageContainer()));
        }
    }
}
