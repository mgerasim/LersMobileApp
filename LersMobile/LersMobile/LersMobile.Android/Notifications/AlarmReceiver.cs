﻿using System;
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
	/// Принимает уведомления от Alarm manager. В обработчике 
	/// запрашивает новые уведомления от сервера и отображает их в статус-баре.
	/// </summary>
	[BroadcastReceiver]
	class AlarmReceiver : BroadcastReceiver
	{
		/// <summary>
		/// Периодически вызывается в фоне.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="intent"></param>
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

		/// <summary>
		/// Отображает новое уведомление в строке состояния Android.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="notification"></param>
		private void ShowNotification(Context context, Lers.Notification notification)
		{
			// https://forums.xamarin.com/discussion/69009/notification-click-to-run-activity

			// По взаимодействию с формами.
			// https://stackoverflow.com/questions/34754149/android-xamarin-make-push-notification-not-create-a-new-activity-but-use-the-cur
			var intent = new Intent(context, typeof(MainActivity));
			intent.PutExtra("NotificationId", notification.Id);

			var notificationBuilder = new Notification.Builder(context)
				.SetSmallIcon(Resource.Drawable.close_button)
				.SetContentTitle(notification.Type.GetDescription())
				.SetContentText(notification.Message)
				.SetStyle(new Notification.BigTextStyle());
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
				.SetContentText(message)
				.SetStyle(new Notification.BigTextStyle());

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