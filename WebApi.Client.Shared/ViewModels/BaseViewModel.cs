﻿using MvvmCross.Core.ViewModels;
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
    }

    public abstract class BaseViewModel<T> : MvxViewModel<T>
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
    }
}
