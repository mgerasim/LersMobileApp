using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;

namespace LersMobile.Core
{
	/// <summary>
	/// Содержит кроссплатформенные методы для 
	/// получения новых уведомлений пользователя.
	/// </summary>
    public static class NotificationChecker
    {
		public static async Task CheckNewNotifications(Action<Lers.Notification> handler)
		{
			var storageServie = DependencyService.Get<IAppDataStorage>();

			if (string.IsNullOrEmpty(storageServie.Token)
				|| string.IsNullOrEmpty(storageServie.ServerAddress))
			{
				return;
			}

			var appService = new Core.MobileCore();

			// Подключаемся к серверу.
			await appService.ConnectToken(storageServie.ServerAddress, storageServie.Token);

			Lers.Notification[] newNotifications = null;

			try
			{
				// Получаем дату последнего уведомления.

				DateTime lastNotifyDate = storageServie.LastNotifyDate;

				// Получаем список новых уведомлений.
				newNotifications = await GetNewNotifications(appService, lastNotifyDate);
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
					// Так же пропускаем те уведомления, которые уже прочитаны.

					foreach (var notification in newNotifications
						.Where(x => x.Id > lastNotifyId && !x.IsRead))
					{
						handler(notification);
					}
				}

				storageServie.LastNotifyDate = newNotifications.First().DateTime;
				storageServie.LastNotifyId = newNotifications.First().Id;

				storageServie.Save();
			}
		}

		private static async Task<Lers.Notification[]> GetNewNotifications(Core.MobileCore appService, DateTime lastNotifyDate)
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

			var list = await getTask;

			return list.OrderByDescending(x => x.Id).ToArray();
		}
    }
}
