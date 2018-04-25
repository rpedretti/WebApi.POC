using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Client.Shared.Services;
using Windows.Storage.Streams;
using Windows.System.Profile;

namespace WebApi.UWP.Services
{
    public sealed class DeviceInformationService : IDeviceInformationService
    {
        public string DeviceId
        {
            get
            {
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
                {
                    var token = HardwareIdentification.GetPackageSpecificToken(null);
                    var hardwareId = token.Id;
                    var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                    byte[] bytes = new byte[hardwareId.Length];
                    dataReader.ReadBytes(bytes);

                    return BitConverter.ToString(bytes).Replace("-", "");
                }

                throw new Exception("NO API FOR DEVICE ID PRESENT!");
            }
        }
    }
}