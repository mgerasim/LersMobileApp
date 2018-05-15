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

		public MainPage()
		{
			lersService = App.Core;

			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var notifications = await lersService.GetNotifications();
		}
	}
}
