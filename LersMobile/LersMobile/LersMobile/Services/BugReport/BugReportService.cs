using LersMobile.Pages.BugPage;
using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Services.BugReport
{
	/// <summary>
	/// Реализация функция по обработке ошибок в работе приложения
	/// </summary>
    public static class BugReportService
    {
		/// <summary>
		/// Обработка исключений
		/// </summary>
		/// <param name="title"></param>
		/// <param name="exception"></param>
		/// <param name="ex"></param>
		public static void HandleException(string title, string exception, Exception ex)
		{
			((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new BugPage(title, exception, ex));
		}
    }
}
