﻿using LersMobile.Droid;
using LersMobile.Services.Device;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceAndroidService))]
namespace LersMobile.Droid
{
	/// <summary>
	/// Реализация функциональности для работы с устройством
	/// </summary>
	public class DeviceAndroidService : IDeviceService
	{
		/// <summary>
		/// Получение идентификатора устройства
		/// </summary>
		/// <returns></returns>
		public string GetIdentifier()
		{
			Android.Telephony.TelephonyManager mTelephonyMgr;
			mTelephonyMgr = (Android.Telephony.TelephonyManager)Forms.Context.GetSystemService(Android.Content.Context.TelephonyService);
			return mTelephonyMgr.DeviceId;
		}

		/// <summary>
		/// Получение архитектуры процессора
		/// </summary>
		/// <returns></returns>
		/// <remarks>https://stackoverflow.com/questions/37112544/how-to-identify-processor-architecture-in-xamarin-android</remarks>
		public string GetProcessorArchitecture()
		{
			string osArchitecture = Java.Lang.JavaSystem.GetProperty("os.arch");
			return osArchitecture;
		}
	}
}