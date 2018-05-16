using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LersMobile
{
    static class IndicatorExtensions
    {
		public static void Show(this ActivityIndicator indicator)
		{
			indicator.IsRunning = true;
			indicator.IsVisible = true;
		}

		public static void Hide(this ActivityIndicator indicator)
		{
			indicator.IsRunning = false;
			indicator.IsVisible = false;
		}
	}
}
