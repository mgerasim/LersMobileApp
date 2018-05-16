using Lers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LersMobile.Core
{
    public class MobileCore
    {
		private LersServer server;

		public string ServerAddress { get; set; }

        public string Token { get; set; }


		private readonly IAppDataStorage storageService;

		public MobileCore()
		{
			this.server = new LersServer("Lers android");

			this.server.VersionMismatch += (sender, e) => e.Ignore = true;

			this.storageService = DependencyService.Get<IAppDataStorage>();
		}

		public async Task Connect(string serverAddress, string login, string password)
		{
			this.ServerAddress = serverAddress;

			try
			{
				var token = await this.server.ConnectAsync(serverAddress, 10000, null,
					new Lers.Networking.BasicAuthenticationInfo(login, Lers.Networking.SecureStringHelper.ConvertToSecureString(password)));

				this.storageService.Token = token.Token;
				this.storageService.ServerAddress = ServerAddress;

				this.storageService.Save();
			}
			catch (Lers.Networking.AuthorizationFailedException)
			{
				// Произошла ошибка аутентификации. Нужно очистить токен и выдать ошибку.
				this.storageService.Token = null;
				this.storageService.Save();

				throw;
			}
		}

		public Task<Notification[]> GetNotifications()
		{
			return this.server.Notifications.GetListAsync();
		}

		private (string, ushort) SplitAddressPort()
		{
			throw new NotImplementedException();
		}
	}
}
