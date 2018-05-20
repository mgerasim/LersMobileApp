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

		/// <summary>
		/// Выбранная для отображения группа объектов.
		/// </summary>
		int? SelectedGroupId { get; set; }

		/// <summary>
		/// Дата последнего уведомления.
		/// </summary>
		DateTime LastNotifyDate { get; set; }

		/// <summary>
		/// Идентификатор последнего уведомления.
		/// </summary>
		long LastNotifyId { get; set; }
	}
}
