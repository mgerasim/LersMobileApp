namespace LersMobile.Services.StorageDirectory
{
	/// <summary>
	/// Интерфейс получения директории для сохранения файлов
	/// </summary>
    public interface IStorageDirectoryService
    {
		/// <summary>
		/// Получение полного имени каталога
		/// </summary>
		/// <returns></returns>
		string Get();
    }
}
