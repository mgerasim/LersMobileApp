using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LersMobile
{
	public partial class MainPage : ContentPage
	{
		private readonly Core.MobileCore lersService;

		private bool isLoaded = false;

		private Lers.LersServer server => this.lersService.Server;

		private bool _isRefreshing = false;

		public bool IsRefreshing
		{
			get { return _isRefreshing; }
			set
			{
				_isRefreshing = value;
				OnPropertyChanged(nameof(IsRefreshing));
			}
		}


		public ICommand RefreshCommand
		{
			get
			{
				return new Command(async () =>
				{
					await RefreshData();
				});
			}
		}

		public MainPage()
		{
			lersService = App.Core;

			InitializeComponent();

			//https://xamarinhelp.com/pull-to-refresh-listview/

			this.nodeListView.RefreshCommand = this.RefreshCommand;
		}


		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isLoaded)
			{
				return;
			}

			await this.lersService.EnsureConnected();

			this.nodeListView.BeginRefresh();

			this.isLoaded = true;
		}

		private async Task RefreshData()
		{
			this.IsRefreshing = true;

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
				this.IsRefreshing = false;
			}
		}
	}
}
