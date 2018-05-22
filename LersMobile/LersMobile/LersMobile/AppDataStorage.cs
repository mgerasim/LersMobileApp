using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace LersMobile
{	
	/// <summary>
	/// Реализация интерфейса хранения данных приложения.
	/// </summary>
	public class AppDataStorage 
	{
		private static ISettings Settings => CrossSettings.Current;

		public static string Token
		{
			get => Settings.GetValueOrDefault(nameof(Token), string.Empty);
			set => Settings.AddOrUpdateValue(nameof(Token), value);
		}

		public static string ServerAddress
		{
			get => Settings.GetValueOrDefault(nameof(ServerAddress), string.Empty);
			set => Settings.AddOrUpdateValue(nameof(ServerAddress), value);
		}

		public static int SelectedGroupId
		{
			get => Settings.GetValueOrDefault(nameof(SelectedGroupId), -1);
			set => Settings.AddOrUpdateValue(nameof(SelectedGroupId), value);
		}


		public static DateTime LastNotifyDate
		{
			get => Settings.GetValueOrDefault(nameof(LastNotifyDate), DateTime.MinValue);
			set => Settings.AddOrUpdateValue(nameof(LastNotifyDate), value);
		}

		public static long LastNotifyId
		{
			get => Settings.GetValueOrDefault(nameof(LastNotifyId), (long)-1);
			set => Settings.AddOrUpdateValue(nameof(LastNotifyId), value);
		}
	}
}