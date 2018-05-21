using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Lers;
using Lers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
			_task = new Task(DoWork);
		}

		public override void OnDestroy()
		{
			_isRunning = false;

			if (_task != null)
			{
				_task.Dispose();
			}
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			if (!_isRunning)
			{
				_isRunning = true;
				_task.Start();
			}

			return StartCommandResult.Sticky;
		}

		#endregion

		private void DoWork()
		{
			try
			{
				CheckNewNotifications().Wait();
			}
			catch (Exception e)
			{
			}
			finally
			{
				StopSelf();
			}
		}


		private async Task CheckNewNotifications()
		{
			var storageServie = DependencyService.Get<IAppDataStorage>();

			if (string.IsNullOrEmpty(storageServie.Token)
				|| string.IsNullOrEmpty(storageServie.ServerAddress))
			{
				return;
			}

			var appService = new Core.MobileCore();

			// Подключаемся к серверу.
			appService.ConnectToken(storageServie.ServerAddress, storageServie.Token).Wait();

			Lers.Notification[] newNotifications = null;

			try
			{
				// Получаем дату последнего уведомления.

				DateTime lastNotifyDate = storageServie.LastNotifyDate;

				// Получаем список новых уведомлений.
				newNotifications = GetNewNotifications(appService, lastNotifyDate);
			}
			catch
			{
				// Подключение к серверу больше не требуется.

				appService.Disconnect();
			}


			if (newNotifications != null && newNotifications.Length > 0)
			{
				long lastNotifyId = storageServie.LastNotifyId;

				if (lastNotifyId != 0)
				{
					// Уведомляем обо всех событиях, у которых Id больше чем последний.

					foreach (var notification in newNotifications.Where(x => x.Id > lastNotifyId))
					{
						ShowNotification(notification);
					}
				}

				storageServie.LastNotifyDate = newNotifications.First().DateTime;
				storageServie.LastNotifyId = newNotifications.First().Id;

				storageServie.Save();
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

		private static Lers.Notification[] GetNewNotifications(Core.MobileCore appService, DateTime lastNotifyDate)
		{
			Task<Lers.Notification[]> getTask;

			if (lastNotifyDate == DateTime.MinValue)
			{
				getTask = appService.Server.Notifications.GetListAsync();
			}
			else
			{
				DateTime endDate = DateTime.Now.AddDays(1);

				getTask = appService.Server.Notifications.GetListAsync(lastNotifyDate.AddSeconds(-1), endDate);
			}

			return getTask.Result.OrderByDescending(x => x.Id).ToArray();
		}
	}
}
