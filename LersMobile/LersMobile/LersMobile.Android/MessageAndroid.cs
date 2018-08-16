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

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace LersMobile.Droid
{
    public class MessageAndroid : IMessage
    {
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