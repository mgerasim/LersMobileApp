using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LersMobile.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AppDataStorage))]
namespace LersMobile.Droid
{	
	/// <summary>
	/// Реализация интерфейса хранения данных для андроида.
	/// </summary>
	class AppDataStorage : IAppDataStorage
	{
		public string Token
		{
			get => (string)GetProperty(nameof(Token));
			set => SetProperty(nameof(Token), value);
		}

		public string ServerAddress
		{
			get => (string)GetProperty(nameof(ServerAddress));
			set => SetProperty(nameof(ServerAddress), value);
		}

		public int? SelectedGroupId
		{
			get => (int?)GetProperty(nameof(SelectedGroupId));
			set => SetProperty(nameof(SelectedGroupId), value);
		}


		public DateTime LastNotifyDate
		{
			get => (DateTime?)GetProperty(nameof(LastNotifyDate)) ?? DateTime.MinValue;
			set => SetProperty(nameof(LastNotifyDate), value);
		}

		public long LastNotifyId
		{
			get => (long?)GetProperty(nameof(LastNotifyId)) ?? 0;
			set => SetProperty(nameof(LastNotifyId), value);
		}

		private object GetProperty(string keyName)
		{
			if (Xamarin.Forms.Application.Current.Properties.TryGetValue(keyName, out var value))
			{
				return value;
			}

			return null;
		}

		private void SetProperty(string keyName, object value) => Xamarin.Forms.Application.Current.Properties[keyName] = value;

		public void Save() => Xamarin.Forms.Application.Current.SavePropertiesAsync();
	}
}