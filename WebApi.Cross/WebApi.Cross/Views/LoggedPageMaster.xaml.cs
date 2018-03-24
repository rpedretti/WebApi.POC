using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebApi.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoggedPageMaster : ContentPage
    {
        public ListView ListView;

        public LoggedPageMaster()
        {
            InitializeComponent();

            BindingContext = new LoggedPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class LoggedPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<LoggedPageMenuItem> MenuItems { get; set; }
            
            public LoggedPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<LoggedPageMenuItem>(new[]
                {
                    new LoggedPageMenuItem { Id = 0, Title = "Page 1" },
                    new LoggedPageMenuItem { Id = 1, Title = "Page 2" },
                    new LoggedPageMenuItem { Id = 2, Title = "Page 3" },
                    new LoggedPageMenuItem { Id = 3, Title = "Page 4" },
                    new LoggedPageMenuItem { Id = 4, Title = "Page 5" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}