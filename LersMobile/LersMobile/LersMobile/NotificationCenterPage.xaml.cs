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

			this.notificationCenterListView.ItemSelected += NotificationCenterListView_ItemSelected;

			this.BindingContext = this;
		}

		/// <summary>
		/// Обрабатывает выбор уведомления в списке.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void NotificationCenterListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = (Core.NotificationDetail)e.SelectedItem;

			if (item != null)
			{
				// Откроем свойства уведомления
				await this.Navigation.PushAsync(new NotificationInfoPage(item));

				// Маркируем уведомление как прочитанное.
				await item.MarkAsReadAsync();
			}

			this.notificationCenterListView.SelectedItem = null;
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