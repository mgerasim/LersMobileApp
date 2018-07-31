using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
	/// <summary>
	/// Содержит вспомогательные методы.
	/// </summary>
	static class Helper
	{
		/// <summary>
		/// Отображает в приложении информацию об уведомлении, открытую из "шторки"
		/// </summary>
		/// <param name="NotificationId">Идентификатор уведомления</param>
		/// <returns></returns>
		public static void ShowNotificationInfoPage(int NotificationId)
		{
			App.NotificationId = NotificationId;

			var item = new MainPageMenuItem() { Id = 2, Title = Droid.Resources.Messages.MainPage_MenuItem_NotificationList, TargetType = typeof(NotificationCenterPage) };

			((MainPage)Xamarin.Forms.Application.Current.MainPage).SwitchDetailToItem(item);
		}
	}
}
