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
		private readonly Core.MobileCore lersService;

		private bool isLoaded = false;

		private Lers.LersServer server => this.lersService.Server;

		public MainPage()
		{
			lersService = App.Core;

			InitializeComponent();

			//https://xamarinhelp.com/pull-to-refresh-listview/
		}


		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isLoaded)
			{
				return;
			}

			await RefreshData();

			this.isLoaded = true;
		}

		private async Task RefreshData()
		{
			this.busyIndicator.Show();

			try
			{
				await this.lersService.EnsureConnected();

				var nodes = await this.server.Nodes.GetListAsync();

				this.BindingContext = nodes;
			}
			catch (Exception exc)
			{
				// TODO: show error
			}
			finally
			{
				this.busyIndicator.Hide();
			}
		}
	}
}
