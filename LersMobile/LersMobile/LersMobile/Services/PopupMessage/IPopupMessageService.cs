using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Services.PopupMessage
{
	/// <summary>
	/// Интерфейс кросс платформенной реализации всплывающих уведомлений
	/// </summary>
	public interface IPopupMessageService
    {
		/// <summary>
		/// Показать всплывающее уведомление с заданным текстом text и длительность isLong
		/// </summary>
		/// <param name="text"></param>
		/// <param name="isLong"></param>
		void Show(string text, bool isLong = false);
	}
}
