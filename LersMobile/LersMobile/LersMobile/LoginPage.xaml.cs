using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
		}

		public async void OnLogin()
		{
			var serverAddressInput = this.FindByName<Entry>("serverAddressInput");
			var loginInput = this.FindByName<Entry>("loginInput");
			var passwordInput = this.FindByName<Entry>("passwordInput");

			try
			{
				this.ShowIndicator("busyIndicator");

				await App.Core.Connect(serverAddressInput.Text, loginInput.Text, passwordInput.Text);

				this.HideIndicator("busyIndicator");

				RedirectToMainPage();
			}
			catch (Exception exc)
			{
				// TODO: для отображение сообщений нужно использовать DependancyService, т.к. Toast.MakeText специфичен для android.
				// https://stackoverflow.com/questions/35279403/toast-equivalent-on-xamarin-forms
				// https://xamarinhelp.com/toast-notifications-xamarin-forms/
				Toast.MakeText(Android.App.Application.Context, "Ошибка подключения к серверу. " + exc.Message, ToastLength.Short)
					.Show();
			}
			finally
			{
				this.HideIndicator("busyIndicator");
			}
		}

		private void RedirectToMainPage()
		{
			App.Current.MainPage = new NavigationPage(new MainPage());
		}
	}
}