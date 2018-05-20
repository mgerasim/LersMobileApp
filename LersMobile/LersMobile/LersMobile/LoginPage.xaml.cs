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
		private readonly Core.MobileCore coreService;
		private readonly IAppDataStorage storageService;


		public LoginPage()
		{
			this.coreService = App.Core;
			this.storageService = DependencyService.Get<IAppDataStorage>();

			InitializeComponent();

			this.serverAddressInput.Text = this.storageService.ServerAddress ?? string.Empty;
		}

		protected override async void OnAppearing()
		{
			if (!string.IsNullOrEmpty(this.storageService.ServerAddress) && !string.IsNullOrEmpty(this.storageService.Token))
			{
				this.busyIndicator.Show();

				try
				{
					// Скрываем логин и пароль, так как мы входим по токену.
					ShowPasswordControls(false);

					await this.coreService.ConnectToken(this.storageService.ServerAddress, this.storageService.Token);

					RedirectToMainPage();
				}
				catch (Exception exc)
				{
					// Произошла ошибка входа, попробуем войти с помощью логина и пароля.

					ShowPasswordControls(true);

					await DisplayAlert("Ошибка подключения", exc.Message, "OK");

					// TODO: для отображение сообщений нужно использовать DependancyService, т.к. Toast.MakeText специфичен для android.

					// https://stackoverflow.com/questions/35279403/toast-equivalent-on-xamarin-forms
					// https://xamarinhelp.com/toast-notifications-xamarin-forms/
				}
				finally
				{
					this.busyIndicator.Hide();
				}
			}
		}

		private void ShowPasswordControls(bool show)
		{
			this.loginInput.IsVisible = show;
			this.passwordInput.IsVisible = show;
			this.loginLabel.IsVisible = show;
			this.passwordLabel.IsVisible = show;
			this.onLogin.IsVisible = show;
		}

		public async void OnLogin()
		{
			if (!CheckUserInput())
			{
				return;
			}

			try
			{
				this.busyIndicator.Show();

				await App.Core.Connect(serverAddressInput.Text, loginInput.Text, passwordInput.Text);

				this.busyIndicator.Hide();

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
				this.busyIndicator.Hide();
			}
		}

		private bool CheckUserInput()
		{
			if (string.IsNullOrEmpty(serverAddressInput.Text))
			{
				Toast.MakeText(Android.App.Application.Context, "Не задан адрес сервера.", ToastLength.Short)
					.Show();

				return false;
			}

			if (string.IsNullOrEmpty(this.loginInput.Text))
			{
				Toast.MakeText(Android.App.Application.Context, "Не задано имя входа.", ToastLength.Short)
					.Show();
				return false;
			}

			if (string.IsNullOrEmpty(this.passwordInput.Text))
			{
				Toast.MakeText(Android.App.Application.Context, "Не задан пароль.", ToastLength.Short)
					.Show();
				return false;
			}

			return true;
		}

		private void RedirectToMainPage()
		{
			App.Current.MainPage = new MainPage();
		}
	}
}