using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
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

					await DisplayAlert(LersMobile.Droid.Resources.Messages.ErrorConnect, exc.Message, "OK");
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
                
                bool acceptSSL = this.acceptSsl.IsToggled;

                await App.Core.Connect(serverAddressInput.Text, loginInput.Text, passwordInput.Text, acceptSSL);

				this.IsBusy = false;

				SuccessLogin?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception exc)
			{
                await DisplayAlert(LersMobile.Droid.Resources.Messages.ErrorConnectServer, exc.Message, "OK");
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
				Toast.MakeText(Android.App.Application.Context, LersMobile.Droid.Resources.Messages.EmptyServer, ToastLength.Short)
					.Show();

				return false;
			}

			if (string.IsNullOrEmpty(this.loginInput.Text))
			{
				Toast.MakeText(Android.App.Application.Context, LersMobile.Droid.Resources.Messages.EmptyLogin, ToastLength.Short)
					.Show();
				return false;
			}

			if (string.IsNullOrEmpty(this.passwordInput.Text))
			{
				Toast.MakeText(Android.App.Application.Context, LersMobile.Droid.Resources.Messages.EmptyPassword, ToastLength.Short)
					.Show();
				return false;
			}

			return true;
		}
	}
}