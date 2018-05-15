using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LersMobile
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
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
			}
			catch (Exception exc)
			{
				Toast.MakeText(Android.App.Application.Context, "Ошибка подключения к серверу. " + exc.Message, ToastLength.Short);
			}
			finally
			{
				this.HideIndicator("busyIndicator");
			}
		}
	}
}
