using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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


		private Core.NotificationView[] _notifications;

		/// <summary>
		/// Отображаемые уведомления.
		/// </summary>
		public Core.NotificationView[] Notifications
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
        /// Команда, вызываемая из списка для обновления данных.
        /// </summary>
        public ICommand RefreshListView => new Command(async () => await RefreshNotifications());

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
			var item = (Core.NotificationView)e.SelectedItem;

			if (item != null)
			{
				// Откроем свойства уведомления
				await this.Navigation.PushAsync(new NotificationInfoPage(item));

				if (!item.Notification.IsRead)
				{
					try
					{
						// Маркируем уведомление как прочитанное.
						await item.MarkAsReadAsync();
					}
					catch (Exception exc)
					{
						// TODO: всплывающие уведомления нужно показывать через DependencyService,
						// так как они специфичины для Андроида.

						Android.Widget.Toast.MakeText(Android.App.Application.Context,
							exc.Message,
							Android.Widget.ToastLength.Short).Show();
					}
				}
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

            await RefreshNotifications();
        }


        /// <summary>
        /// Обновляет список уведомлений.
        /// </summary>
        /// <returns></returns>
        private async Task RefreshNotifications()
        {
            this.IsRefreshing = true;

            try
            {
                this.Notifications = await this.lersService.GetNotifications();

                this.isLoaded = true;

            }
            catch (Exception exc)
            {
                await DisplayAlert("Ошибка", "Не удалось загрузить уведомления. " + exc.Message, "OK");
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }
    }
}