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
	/// Центр уведомлений пользователя.
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationCenterPage : ContentPage
	{
		private readonly Core.MobileCore lersService;

		private bool isLoaded = false;


		private Core.NotificationDetail[] _notifications;

		/// <summary>
		/// Отображаемые уведомления.
		/// </summary>
		public Core.NotificationDetail[] Notifications
		{
			get => _notifications;
			set
			{
				_notifications = value;
				OnPropertyChanged(nameof(Notifications));
			}
		}

		private bool _isRefreshing = false;

		/// <summary>
		/// Флаг указывает что идёт обновление данных.
		/// </summary>
		public bool IsRefreshing
		{
			get { return _isRefreshing; }
			set
			{
				_isRefreshing = value;
				OnPropertyChanged(nameof(IsRefreshing));
			}
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		public NotificationCenterPage()
		{
			InitializeComponent();

			this.lersService = App.Core;

			this.BindingContext = this;
		}

		/// <summary>
		/// Вызывается при отображении страницы на экране.
		/// </summary>
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isLoaded)
			{
				return;
			}

			this.IsRefreshing = true;

			try
			{
				var notifications = await this.lersService.GetNotifications();
				this.Notifications = notifications.OrderByDescending(x => x.DateTime).ToArray();
			}
			finally
			{
				this.IsRefreshing = false;
			}

			this.isLoaded = true;
		}
	}
}