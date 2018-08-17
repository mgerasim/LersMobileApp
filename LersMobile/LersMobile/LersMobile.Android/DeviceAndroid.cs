using LersMobile.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceAndroid))]
namespace LersMobile.Droid
{
	public class DeviceAndroid : IDevice
	{
		public string GetIdentifier()
		{
			Android.Telephony.TelephonyManager mTelephonyMgr;
			mTelephonyMgr = (Android.Telephony.TelephonyManager)Forms.Context.GetSystemService(Android.Content.Context.TelephonyService);
			return mTelephonyMgr.DeviceId;
		}
	}
}