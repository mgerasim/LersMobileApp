﻿
using Xamarin.Forms;

namespace LersMobile.Services.Device
{
	/// <summary>
	/// Сервис для использования по работе с устройством 
	/// </summary>
    public static class DeviceService
    {
		/// <summary>
		/// Получение идентификатора устройства
		/// </summary>
		/// <returns></returns>
		public static string GetIdentifier()
		{
			return DependencyService.Get<IDeviceService>().GetIdentifier();
		}
		/// <summary>
		/// Получение архитектуры процессор устройства
		/// </summary>
		/// <returns></returns>
		public static string GetProcessorArchitecture()
		{
			return DependencyService.Get<IDeviceService>().GetProcessorArchitecture();
		}

	}
}
