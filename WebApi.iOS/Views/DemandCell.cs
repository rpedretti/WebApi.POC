using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;
using WebApi.Client.Shared.Models;

namespace WebApi.iOS.Views
{
    public partial class DemandCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("DemandCell");
        public static readonly UINib Nib;

        static DemandCell()
        {
            Nib = UINib.FromName("DemandCell", NSBundle.MainBundle);
        }

        protected DemandCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            this.DelayBind(() => {
                var set = this.CreateBindingSet<DemandCell, DemandListItem>();
                set.Bind(Description).To(d => d.Description).OneWay();
                set.Apply();
            });
        }
    }
}
