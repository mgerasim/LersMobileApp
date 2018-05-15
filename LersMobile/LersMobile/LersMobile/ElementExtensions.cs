using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LersMobile
{
    static class ElementExtensions
    {
		public static void ShowIndicator(this Element element, string indicatorName)
		{
			var indicator = element.FindByName<ActivityIndicator>(indicatorName);
			indicator.IsRunning = true;
			indicator.IsVisible = true;
		}

		public static void HideIndicator(this Element element, string indicatorName)
		{
			var indicator = element.FindByName<ActivityIndicator>(indicatorName);
			indicator.IsRunning = false;
			indicator.IsVisible = false;
		}
	}
}
