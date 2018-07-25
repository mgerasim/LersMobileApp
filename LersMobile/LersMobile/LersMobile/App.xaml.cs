using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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

            SetCulture("en-US");

            Core = new Core.MobileCore();

			Core.LoginRequired += Core_LoginRequired;

			ShowLoginPage();
		}

        private static void SetCulture(String culture)
        {            
            // Установить ресурс, соответствующий culture      
            var dictionaryList = new List<ResourceDictionary>(Current.Resources.MergedDictionaries);

            String requestedCulture = $"{culture}.xaml";
            ResourceDictionary resourceDictionary = dictionaryList.Find(d => d.Source.OriginalString == "Lang\\" + requestedCulture);
            if (resourceDictionary == null)
            {
                // Культура по умолчанию
                requestedCulture = "en-US.xaml";
                resourceDictionary = dictionaryList.Find(d => d.Source.OriginalString == "Lang\\" + requestedCulture);
            }
  
            if (resourceDictionary != null)
            {
                Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            var cultureInfo = new System.Globalization.CultureInfo(culture);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
        
        private void Core_LoginRequired(object sender, EventArgs e)
		{
			if (MainPage.GetType() != typeof (LoginPage))
			{
				// Возвращаем пользователя на экран входа.
				ShowLoginPage();
			};
		}

		/// <summary>
		/// Отображает страницу подключения к серверу.
		/// </summary>
		public void ShowLoginPage()
		{
			var loginPage = new LoginPage();

			loginPage.SuccessLogin += (sender, e) =>
			{
				// Перенаправляем на главную страницу приложения.
				MainPage = new MainPage();
			};

			MainPage = loginPage;
		}

		public void ShowNotification(int notificationId)
		{

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
