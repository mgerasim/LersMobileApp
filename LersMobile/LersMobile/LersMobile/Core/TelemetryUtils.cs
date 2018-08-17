using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
    public class TelemetryUtils
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
