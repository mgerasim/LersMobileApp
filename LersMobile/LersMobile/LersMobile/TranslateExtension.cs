using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile
{
	/// <summary>
	/// Предназначен для поддержки использования текстовых ресурсов в разметке xaml для свойства Text
	/// </summary>
	/// <remarks>
	/// Требует подключение:
	/// xmlns:local="clr-namespace:LersMobile"
	/// Пример использования:
	/// <Label Text="{local:Translate LoginPage_Server}" />
	/// где LoginPage_Server - текстовый ресурс
	/// </remarks>
	[ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
		/// <summary>
		/// Расположение ресурса текстовый ресурс
		/// </summary>
        const string ResourceId = "LersMobile.Droid.Resources.Messages";

		/// <summary>
		/// Свойство элемента в разметке xaml для которого поддерживается применение текстовый ресурс
		/// </summary>
        public string Text { get; set; }

		/// <summary>
		/// Реализует получение строки по текстовому ресурсу в зависимости от локализации 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;
            ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

            return resourceManager.GetString(Text, CultureInfo.CurrentCulture);
        }
    }
}
