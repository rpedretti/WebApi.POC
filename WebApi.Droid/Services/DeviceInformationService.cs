using Android.OS;
using WebApi.Client.Shared.Services;

namespace WebApi.Droid.Services
{
    public sealed class DeviceInformationService : IDeviceInformationService
    {
        public string DeviceId => Build.Serial;
    }
}