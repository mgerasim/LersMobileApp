using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile
{
	/// <summary>
	/// Страница с подробной информацией об уведомлении.
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationInfoPage : ContentPage
	{
		private readonly Core.NotificationView notification;

		public NotificationInfoPage(Core.NotificationView notification)
		{
			InitializeComponent();

			this.notification = notification ?? throw new ArgumentNullException(nameof(notification));

			this.BindingContext = this.notification;

			this.Title = Droid.Resources.Messages.MainPage_MenuItem_NotificationList;
		}
	}
}