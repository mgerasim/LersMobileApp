﻿using LersMobile.Core;
using LersMobile.Services.BugReport;
using LersMobile.Services.PopupMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

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

			Uri uri = new Uri(AppDataStorage.Uri);

			this.serverAddressInput.Text = uri.Host;
            
            this.sslSwitch.IsToggled = uri.Scheme == Lers.LersScheme.Secure;			

            this.loginInput.Text = AppDataStorage.Login;
		}

		protected override async void OnAppearing()
		{
			if (!string.IsNullOrEmpty(AppDataStorage.Uri) && !string.IsNullOrEmpty(AppDataStorage.Token))
			{
				this.IsBusy = true;
				this.onLoginButton.IsEnabled = false;

				try
				{
					// Скрываем логин и пароль, так как мы входим по токену.
					ShowPasswordControls(false);

					await this.coreService.ConnectToken(AppDataStorage.Uri, AppDataStorage.Token);

					SuccessLogin?.Invoke(this, EventArgs.Empty);
				}
				catch (Exception exc)
				{
					// Произошла ошибка входа, попробуем войти с помощью логина и пароля.

					ShowPasswordControls(true);

					BugReportService.HandleException(Droid.Resources.Messages.LoginPage_Error_Connect, exc.Message, exc);
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
			this.sslLabel.IsVisible = show;
			this.sslSwitch.IsVisible = show;
		}

		public async void OnLogin()
		{
			if (this.IsBusy == true)
			{
				// Исключить повторную авторизации
				return;
			}

			if (!CheckUserInput())
			{
				return;
			}

			try
			{
				this.IsBusy = true;
                
                bool acceptSsl = sslSwitch.IsToggled;

                await App.Core.Connect(serverAddressInput.Text, loginInput.Text, passwordInput.Text, acceptSsl);

				this.IsBusy = false;

				SuccessLogin?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception exc)
			{
				BugReportService.HandleException(Droid.Resources.Messages.LoginPage_Error_Connect_to_Server, exc.Message, exc);
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
				PopupMessageService.ShowShort(Droid.Resources.Messages.LoginPage_Empty_Server);

				return false;
			}

			if (string.IsNullOrEmpty(this.loginInput.Text))
			{
				PopupMessageService.ShowShort(Droid.Resources.Messages.LoginPage_Empty_Login);
				return false;
			}

			if (string.IsNullOrEmpty(this.passwordInput.Text))
			{
				PopupMessageService.ShowShort(Droid.Resources.Messages.LoginPage_Empty_Password);
				return false;
			}

			return true;
		}
	}
}