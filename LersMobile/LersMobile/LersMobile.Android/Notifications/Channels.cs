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

namespace LersMobile.Droid.Notifications
{
	/// <summary>
	/// Содержит методы для управления каналами уведомлений.
	/// </summary>
	static class Channels
	{
		public static string GeneralChannelId => "com.lers.app.general_channel";

		public static string ChannelName = "Центр уведомлений";

		/// <summary>
		/// Создаёт канал для отправки уведомлений.
		/// </summary>
		/// <param name="context"></param>
		public static void CreateChannel(Context context)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.O)
			{
				// Каналы поддерживаются только на OREO и выше.
				return;
			}

			var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);

			var existing = notificationManager.GetNotificationChannel(GeneralChannelId);

			if (existing == null)
			{
				return;
			}

			var importance = NotificationImportance.High;
			var channel = new NotificationChannel(GeneralChannelId, ChannelName, importance);

			channel.EnableVibration(true);
			channel.LockscreenVisibility = NotificationVisibility.Public;

			notificationManager.CreateNotificationChannel(channel);
		}
	}
}