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
		private readonly Core.NotificationDetail notification;

		public NotificationInfoPage(Core.NotificationDetail notification)
		{
			InitializeComponent();

			this.notification = notification ?? throw new ArgumentNullException(nameof(notification));

			this.BindingContext = this.notification;
		}
	}
}