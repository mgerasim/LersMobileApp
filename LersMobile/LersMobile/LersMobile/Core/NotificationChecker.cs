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
			if (string.IsNullOrEmpty(AppDataStorage.Token)
				|| string.IsNullOrEmpty(AppDataStorage.ServerAddress))
			{
				return;
			}

			var appService = new MobileCore();

			// Подключаемся к серверу.
			await appService.ConnectToken(AppDataStorage.ServerAddress, AppDataStorage.Token);

			Lers.Notification[] newNotifications = null;

			try
			{
				// Получаем дату последнего уведомления.

				DateTime lastNotifyDate = AppDataStorage.LastNotifyDate;

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

#if (DEBUG)
				{
					handler(newNotifications.Last());					
				}
#endif
				long lastNotifyId = AppDataStorage.LastNotifyId;

				if (lastNotifyId > 0)
				{
					// Уведомляем обо всех событиях, у которых Id больше чем последний.
					// Так же пропускаем те уведомления, которые уже прочитаны.

					foreach (var notification in newNotifications
						.Where(x => x.Id > lastNotifyId && !x.IsRead))
					{
						handler(notification);
					}
				}

				AppDataStorage.LastNotifyDate = newNotifications.First().DateTime;
				AppDataStorage.LastNotifyId = newNotifications.First().Id;
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
