using Android.App;
using Android.Content;
using Android.OS;
using Lers.Utils;
using System;
using System.Threading.Tasks;

namespace LersMobile.Droid.Notifications
{
	/// <summary>
	/// Сервис, получающий новые уведомления.
	/// </summary>
	/// <remarks>
	/// Код взят здесь
	// https://github.com/wcoder/PeriodicBackgroundService/tree/master/PeriodicBackgroundService/PeriodicBackgroundService.Android
	/// </remarks>
	[Service]
	public class NotificationService : Service
	{
		private const string Tag = "[PeriodicBackgroundService]";

		private bool _isRunning;
		private Context _context;
		private Task _task;

		#region overrides

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		public override void OnCreate()
		{
			_context = this;
			_isRunning = false;
		}

		public override void OnDestroy()
		{
			_isRunning = false;

			if (_task != null)
			{
				try
				{
					_task.Wait();
				}
				catch
				{
				}

				_task.Dispose();
			}
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			if (!_isRunning)
			{
				_isRunning = true;
				_task = RunNewNotificationsCheck();
			}

			return StartCommandResult.Sticky;
		}

		#endregion

		private async Task RunNewNotificationsCheck()
		{
			try
			{
				await Core.NotificationChecker.CheckNewNotifications(ShowNotification);
			}
			catch (Exception exc)
			{
				ShowNotification("Error", exc.Message);
			}
			finally
			{
				StopSelf();
			}
		}


		private void ShowNotification(Lers.Notification notification)
		{
			var notificationBuilder = new Android.App.Notification.Builder(this)
				.SetSmallIcon(Resource.Drawable.close_button)
				.SetContentTitle(notification.Type.GetDescription())
				.SetContentText(notification.Message);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				// Каналы поддерживаются только на OREO и выше.
				notificationBuilder.SetChannelId(Channels.GeneralChannelId);
			}

			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			notificationManager.Notify(notification.Id, notificationBuilder.Build());
		}

		/// <summary>
		/// DEBUG ONLY
		/// </summary>
		/// <param name="header"></param>
		/// <param name="message"></param>
		private void ShowNotification(string header, string message)
		{
			var notificationBuilder = new Notification.Builder(this)
				.SetSmallIcon(Resource.Drawable.close_button)
				.SetContentTitle(header)
				.SetContentText(message);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				// Каналы поддерживаются только на OREO и выше.
				notificationBuilder.SetChannelId(Channels.GeneralChannelId);
			}

			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			notificationManager.Notify(111, notificationBuilder.Build());
		}
	}
}
