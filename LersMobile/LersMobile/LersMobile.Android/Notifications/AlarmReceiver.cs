using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Lers.Utils;

namespace LersMobile.Droid.Notifications
{
	/// <summary>
	/// Принимает уведомления от Alarm manager.
	/// </summary>
	[BroadcastReceiver]
	class AlarmReceiver : BroadcastReceiver
	{
		public override async void OnReceive(Context context, Intent intent)
		{
			await RunNewNotificationsCheck(context);
		}

		private async Task RunNewNotificationsCheck(Context context)
		{
			try
			{
				await Core.NotificationChecker.CheckNewNotifications((n) => ShowNotification(context, n));
			}
			catch (Exception exc)
			{
				ShowNotification(context, "Error", exc.Message);
			}
		}

		private void ShowNotification(Context context, Lers.Notification notification)
		{
			// https://forums.xamarin.com/discussion/69009/notification-click-to-run-activity
			var intent = new Intent(context, typeof(MainActivity));
			intent.PutExtra("NotificationId", notification.Id);

			var notificationBuilder = new Notification.Builder(context)
				.SetSmallIcon(Resource.Drawable.close_button)
				.SetContentTitle(notification.Type.GetDescription())
				.SetContentText(notification.Message);
				// .SetContentIntent(intent);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				// Каналы поддерживаются только на OREO и выше.
				notificationBuilder.SetChannelId(Channels.GeneralChannelId);
			}

			var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
			notificationManager.Notify(notification.Id, notificationBuilder.Build());
		}

		/// <summary>
		/// DEBUG ONLY
		/// </summary>
		private void ShowNotification(Context context, string header, string message)
		{
			var notificationBuilder = new Notification.Builder(context)
				.SetSmallIcon(Resource.Drawable.close_button)
				.SetContentTitle(header)
				.SetContentText(message);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				// Каналы поддерживаются только на OREO и выше.
				notificationBuilder.SetChannelId(Channels.GeneralChannelId);
			}

			var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
			notificationManager.Notify(111, notificationBuilder.Build());
		}
	}
}