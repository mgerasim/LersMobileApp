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
	/// Главное окно приложения, которое представляет собой меню 
	/// с выбором доступных отображаемых данных и детализацию выбранных данных.
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : MasterDetailPage
	{
		public MainPage()
		{
			InitializeComponent();
			MasterPage.ListView.ItemSelected += ListView_ItemSelected;
		}

		/// <summary>
		/// Пользователь выбрал пункт в навигационном меню.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MainPageMenuItem;
			if (item == null)
				return;

			if (item.TargetType != null)
			{
				SwitchDetailToItem(item);
			}

			// Выполняем действие если оно назначено

			item.TargetAction?.Invoke();

			IsPresented = false;
			MasterPage.ListView.SelectedItem = null;
		}

		/// <summary>
		/// Вызывается при отображении страницы на экране.
		/// </summary>
		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (App.NotificationId > 0)
			{
				Core.NotificationUtils.ShowNotificationInfoPage(App.NotificationId);
			}
		}

		public void SwitchDetailToItem(MainPageMenuItem item)
		{
			bool isDisplaying = false;

			var navPage = this.Detail as NavigationPage;

			if (navPage != null)
			{
				if (navPage.RootPage.GetType() == item.TargetType)
				{
					// Эта страница уже отображается
					isDisplaying = true;
				}
			}

			if (!isDisplaying)
			{
				var page = (Page)Activator.CreateInstance(item.TargetType);
				page.Title = item.Title;

				Detail = new NavigationPage(page);
			}
		}
	}
}