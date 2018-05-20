using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace LersMobile
{
	public partial class App : Application
	{
        public static Core.MobileCore Core { get; private set; }

		public App ()
		{
			InitializeComponent();

            Core = new Core.MobileCore();

			Core.LoginRequired += Core_LoginRequired;
			MainPage = new LoginPage();
		}

		private void Core_LoginRequired(object sender, EventArgs e)
		{
			if (MainPage.GetType() != typeof (LoginPage))
			{
				// Возвращаем пользователя на экран входа.
				MainPage = new LoginPage();
			};
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
