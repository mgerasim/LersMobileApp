using LersMobile.Droid;
using LersMobile.Services.StorageDirectory;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(StorageDirectoryAndroidService))]
namespace LersMobile.Droid
{
	/// <summary>
	/// Реализация интерфейса по получению каталога для сохранения файла
	/// </summary>
	public class StorageDirectoryAndroidService : IStorageDirectoryService
	{
		public string Get()
		{
			string externalStorageDirectory = global::Android.OS.Environment.ExternalStorageDirectory.Path;
			string directorySub = global::Android.OS.Environment.DirectoryDownloads;
			return Path.Combine(externalStorageDirectory, directorySub);
		}
	}
}