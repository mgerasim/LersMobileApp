﻿using Android.Widget;
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

		public event EventHandler SuccessLogin;

		public LoginPage()
		{
			this.coreService = App.Core;

			InitializeComponent();

			this.BindingContext = this;

			this.serverAddressInput.Text = AppDataStorage.ServerAddress;
		}

		protected override async void OnAppearing()
		{
			if (!string.IsNullOrEmpty(AppDataStorage.ServerAddress) && !string.IsNullOrEmpty(AppDataStorage.Token))
			{
				this.IsBusy = true;
				this.onLoginButton.IsEnabled = false;

				try
				{
					// Скрываем логин и пароль, так как мы входим по токену.
					ShowPasswordControls(false);

					await this.coreService.ConnectToken(AppDataStorage.ServerAddress, AppDataStorage.Token);

					SuccessLogin?.Invoke(this, EventArgs.Empty);
				}
				catch (Exception exc)
				{
					// Произошла ошибка входа, попробуем войти с помощью логина и пароля.

					ShowPasswordControls(true);

					await DisplayAlert("Ошибка подключения", exc.Message, "OK");
				}
				finally
				{
					this.IsBusy = false;
					this.onLoginButton.IsEnabled = true;
				}
			}
		}

		private void ShowPasswordControls(bool show)
		{
			this.loginInput.IsVisible = show;
			this.passwordInput.IsVisible = show;
			this.loginLabel.IsVisible = show;
			this.passwordLabel.IsVisible = show;
			this.onLoginButton.IsVisible = show;
		}

		public async void OnLogin()
		{
			if (!CheckUserInput())
			{
				return;
			}

			try
			{
				this.IsBusy = true;

				await App.Core.Connect(serverAddressInput.Text, loginInput.Text, passwordInput.Text);

				this.IsBusy = false;

				SuccessLogin?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception exc)
			{
                await DisplayAlert("Ошибка подключения к серверу", exc.Message, "OK");
			}
			finally
			{
				this.IsBusy = false;
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
	}
}