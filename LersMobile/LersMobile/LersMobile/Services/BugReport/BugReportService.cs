using Lers;
using Lers.Networking;
using LersMobile.Pages.BugPage;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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
		public static async void HandleException(string title, string exception, Exception ex)
		{
			if (ex is NoConnectionException || ex is RequestDisconnectException)
			{
				// ошибки, указывающие что при выполнении запроса разорвалась связь с сервером. Их достаточно перехватить и не обрабатывать,
				// т.к. это нормальная ситуация.
				return;
			}

			if (ex is LersException || ex is LersServerException)
			{
				await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error, exception, "Ok");
				return;
			}

			if (App.Current.MainPage is BugPage)
			{
				return;
			}
			
			Page mainPage = null;
			// Определяем корневую страницу
			if (App.Current.MainPage is LoginPage )				
			{
				// Во время авторизации корневая страница является LoginPage
				mainPage = App.Current.MainPage;
			}
			else
			{
				// После авторизации корневая страница определяется в Detail как выбранная из главного меню
				mainPage = ((MainPage)App.Current.MainPage).Detail;				
			}
			// Показываем отчёт об ошибке
			await mainPage.Navigation.PushModalAsync(new BugPage(title, exception, ex));
		}

		/// <summary>
		/// Обработка исключений
		/// </summary>
		/// <param name="title"></param>
		/// <param name="exception"></param>
		/// <param name="ex"></param>
		public static void UnhandledException(this Exception exception)
		{
			try
			{
				HandleException(exception.Message, "", exception);
			}
			catch (Exception ex)
			{

			}
		}
	}
}
