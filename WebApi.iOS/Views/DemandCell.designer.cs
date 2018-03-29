// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace WebApi.iOS.Views
{
    [Register ("DemandCell")]
    partial class DemandCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Description { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Description != null) {
                Description.Dispose ();
                Description = null;
            }
        }
    }
}