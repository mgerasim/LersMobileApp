using Xamarin.Forms;

namespace LersMobile.Services.PopupMessage
{
	/// <summary>
	/// Сервис для показа всплывающих уведомлений
	/// </summary>
    static public class PopupMessageService
    {
		/// <summary>
		/// Показать всплывающее уведомление с указанным текстом по длительности "Короткое"
		/// </summary>
		/// <param name="text"></param>
		static public void ShowShort(string text)
		{
			DependencyService.Get<IPopupMessageService>().Show(text, false);
		}

		/// <summary>
		/// Показать всплывающее уведомление с указанным текстом по длительности "Длинное"
		/// </summary>
		/// <param name="text"></param>
		static public void ShowLong(string text)
		{
			DependencyService.Get<IPopupMessageService>().Show(text, true);
		}
    }
}
