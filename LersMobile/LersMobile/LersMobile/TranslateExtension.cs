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
	/// Предназначен для поддержки использования строковых констант в разметке xaml для свойства Text
	/// </summary>
	/// <remarks>
	/// Требует подключение:
	/// xmlns:local="clr-namespace:LersMobile"
	/// Пример использования:
	/// <Label Text="{local:Translate LoginPage_Server}" />
	/// где LoginPage_Server - строковая константа, вынесенная в ресурс
	/// </remarks>
	[ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
		/// <summary>
		/// Расположение ресурса строковых констант
		/// </summary>
        const string ResourceId = "LersMobile.Droid.Resources.Messages";

		/// <summary>
		/// Свойство элемента в разметке xaml для которого поддерживается применение строковых констант
		/// </summary>
        public string Text { get; set; }

		/// <summary>
		/// Реализует получение строки по строковой константе из ресурса в зависимости от локализации 
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
