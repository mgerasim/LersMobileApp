using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile
{
	/// <summary>
	/// Интерфейс описывает хранение данных.
	/// </summary>
    interface IAppDataStorage
    {
		/// <summary>
		/// Токен, используемый для аутентификации.
		/// </summary>
		string Token { get; set; }

		/// <summary>
		/// Адрес сервера.
		/// </summary>
		string ServerAddress { get; set; }

		/// <summary>
		/// Сохраняет данные приложения.
		/// </summary>
		void Save();
    }
}
