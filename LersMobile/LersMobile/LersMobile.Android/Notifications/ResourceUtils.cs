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
	/// Содержит вспомогательные методы для работы с ресурсами.
	/// </summary>
	static class ResourceUtils
	{
		/// <summary>
		/// Возвращает icon-переменную по важности уведомления для отображения на "шторки".
		/// </summary>
		/// <param name="importance"></param>
		/// <returns></returns>
		public static int GetImageByImportance (Lers.Importance importance)
		{
			switch (importance)
			{
				case Lers.Importance.Info:
					return Resource.Drawable.notify_info;					
				case Lers.Importance.Warn:
					return Resource.Drawable.notify_warning;
				case Lers.Importance.Error:
				default:
					return Resource.Drawable.notify_error;
			}
		}
	}
}