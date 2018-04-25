using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using WebApi.Client.Shared.Services;

namespace WebApi.iOS.Services
{
    public sealed class DeviceInformationService : IDeviceInformationService
    {
        public string DeviceId => UIDevice.CurrentDevice.IdentifierForVendor.AsString();
    }
}