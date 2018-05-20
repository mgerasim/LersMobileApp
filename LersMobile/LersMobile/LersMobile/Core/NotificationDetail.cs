using System;
using System.Collections.Generic;
using Android.Content.Res;
using Lers;
using LersMobile.Droid;
using Xamarin.Forms;

namespace LersMobile.Core
{
    public class NotificationDetail
    {
		public Notification Notification { get; private set; }

		public Color BackgroundColor
		{
			get
			{
				return this.Notification.IsRead
					? Color.Default
					: Color.LightSteelBlue;
			}
		}

		public FontAttributes FontAttribute
		{
			get
			{
				return this.Notification.IsRead
					? FontAttributes.None
					: FontAttributes.Bold;
			}
		}

		public string Message => this.Notification.Message;

		public DateTime DateTime => this.Notification.DateTime;


		internal  NotificationDetail(Notification notification)
		{
			this.Notification = notification ?? throw new ArgumentNullException(nameof(notification));
		}
    }
}
