using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WebApi.Client.Shared.Services;

namespace WebApi.Droid.Services
{
    public sealed class DeviceInformationService : IDeviceInformationService
    {
        public string DeviceId => Build.Serial;
    }
}