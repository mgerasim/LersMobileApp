using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LersMobile.Services.StorageDirectory
{
	/// <summary>
	/// Сервис для использования по получения директории для сохранения файлов
	/// </summary>
    public static class StorageDirectoryService
    {
		/// <summary>
		/// Функция получения полного имени каталога для сохранения
		/// </summary>
		/// <returns></returns>
		public static string Get()
		{
			return DependencyService.Get<IStorageDirectoryService>().Get();
		}
    }
}
