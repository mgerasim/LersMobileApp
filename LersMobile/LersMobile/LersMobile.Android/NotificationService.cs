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

namespace LersMobile
{
	[Service]
	public class NotificationService : Service
	{
		public override IBinder OnBind(Intent intent) => null;

		public override void OnCreate()
		{
			// https://github.com/wcoder/PeriodicBackgroundService/tree/master/PeriodicBackgroundService/PeriodicBackgroundService.Android
			base.OnCreate();
		}

		[return: GeneratedEnum]
		public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		{
			var storageServie = DependencyService.Get<IAppDataStorage>();

			if (string.IsNullOrEmpty(storageServie.Token)
				|| string.IsNullOrEmpty(storageServie.ServerAddress))
			{
				return StartCommandResult.NotSticky;
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

			return StartCommandResult.NotSticky;
		}

		private void ShowNotification(Lers.Notification notification)
		{
			// Work has finished, now dispatch anotification to let the user know.
			var notificationBuilder = new Android.App.Notification.Builder(this)
				//.SetSmallIcon(Resource.Drawable.ic_notification_small_icon)
				.SetContentTitle(notification.Type.GetShortDescription())
				.SetContentText(notification.Message);

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
