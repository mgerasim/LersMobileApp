using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace LersMobile.Droid
{
    [Activity(Label = "LersMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

			var app = new App();

			LoadPrivateData();

            LoadApplication(app);
        }

        private void LoadPrivateData()
        {
            object token;

            if (Xamarin.Forms.Application.Current.Properties.TryGetValue("LoginToken", out token))
            {
                App.Core.Token = (string)token;
            }
        }
    }
}

