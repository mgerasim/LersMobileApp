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
using LersMobile.Droid;
using LersMobile.Services.PopupMessage;

[assembly: Xamarin.Forms.Dependency(typeof(PopupMessageAndroidService))]
namespace LersMobile.Droid
{
	/// <summary>
	/// Реализация всплывающих уведомлений средствами платформы Android
	/// </summary>
    public class PopupMessageAndroidService : IPopupMessageService
	{
		/// <summary>
		/// Показать всплывающее уведомление с указанным текстом
		/// </summary>
		/// <param name="text"></param>
		/// <param name="isLong"></param>
        public void Show(string text, bool isLong = false)
        {
            ToastLength toastLength = ToastLength.Short;

            if (isLong)
            {
                toastLength = ToastLength.Long;
            }

            Toast.MakeText(Application.Context, text, toastLength).Show();
        }
    }
}