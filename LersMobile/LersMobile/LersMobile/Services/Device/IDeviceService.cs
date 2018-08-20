using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Services.Device
{
	/// <summary>
	/// Интерфейс реализации по работе с устрйством
	/// </summary>
    public interface IDeviceService
    {
		/// <summary>
		/// Получение идентификатора устройства
		/// </summary>
		/// <returns></returns>
		string GetIdentifier();
	}
}
