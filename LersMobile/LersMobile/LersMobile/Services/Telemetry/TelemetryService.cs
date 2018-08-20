using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Services.Telemetry
{
	/// <summary>
	/// Реализация функций по работе с сервером телеметрии
	/// </summary>
    public class TelemetryServices
    {
		/// <summary>
		/// Адрес сервера телеметрии.
		/// </summary>
		public static readonly string ServerEndPointAddress =
#if DEBUG
		"http://lab.lers.ru:8011/track";
#else
		"http://telemetry.lers.ru/track";
#endif

	}
}
