using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using LersMobile.Droid.Notifications;

namespace LersMobile.Droid
{
    [Activity(Label = "LersMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());

			InitNotificationServices(this);
        }

		public static void InitNotificationServices(Context context)
		{
			Channels.CreateChannel(context);
			SetAlarmForBackgroundServices(context);
		}

		private static void SetAlarmForBackgroundServices(Context context)
		{
			var alarmIntent = new Intent(context.ApplicationContext, typeof(AlarmReceiver));
			var broadcast = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.NoCreate);
			if (broadcast == null)
			{
				var pendingIntent = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, 0);
				var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

				alarmManager.SetRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime(), 1 * 60 * 1000, pendingIntent);
			}
		}
	}
}

