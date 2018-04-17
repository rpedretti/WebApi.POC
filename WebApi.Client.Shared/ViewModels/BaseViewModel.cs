using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using System.Threading.Tasks;
using WebApi.Client.Shared.Interactions;

namespace WebApi.Client.Shared.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        protected MvxInteraction<StatusInteraction> _statusMessageInteraction = new MvxInteraction<StatusInteraction>();
        public MvxInteraction<StatusInteraction> StatusMessageInteraction => _statusMessageInteraction;

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            System.Diagnostics.Debug.WriteLine("ViewDestroy");
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            System.Diagnostics.Debug.WriteLine("ViewDisappeared");
        }
    }

    public abstract class BaseViewModel<T> : BaseViewModel, IMvxViewModel<T>
    {
        public async Task Init(string parameter)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                if (!Mvx.TryResolve(out IMvxJsonConverter serializer))
                {
                    throw new MvxIoCResolveException("There is no implementation of IMvxJsonConverter registered. You need to use the MvvmCross Json plugin or create your own implementation of IMvxJsonConverter.");
                }

                var deserialized = serializer.DeserializeObject<T>(parameter);
                Prepare(deserialized);
                await Initialize();
            }
        }

        public abstract void Prepare(T param);
    }
}
