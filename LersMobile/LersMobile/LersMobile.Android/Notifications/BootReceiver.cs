using Android.App;
using Android.Content;

namespace LersMobile.Droid
{
	/// <summary>
	/// Принимает уведомления об окончании загрузки устройства.
	/// </summary>
	[BroadcastReceiver]
	[IntentFilter(new[] { Intent.ActionBootCompleted })]
	public class BootBroadcast : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			MainActivity.InitNotificationServices(context);
		}
	}
}