using LersMobile.Pages.BugPage;
using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
    public static class ExceptionUtils
    {
		public static void HandleException(string title, string exception, Exception ex)
		{
			((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new BugPage(title, exception, ex));
		}
    }
}
