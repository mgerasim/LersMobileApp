using Lers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LersMobile.Core
{
    public class MobileCore
    {
		private LersServer server;

		private readonly IAppDataStorage storageService;

		public LersServer Server => this.server;

		public MobileCore()
		{
			this.server = new LersServer("Lers android");

			this.server.VersionMismatch += (sender, e) => e.Ignore = true;

			this.storageService = DependencyService.Get<IAppDataStorage>();
		}

		public async Task Connect(string serverAddress, string login, string password)
		{
			try
			{
				var securePassword = Lers.Networking.SecureStringHelper.ConvertToSecureString(password);

				var loginInfo = new Lers.Networking.BasicAuthenticationInfo(login, securePassword)
				{
					GetSessionRestoreToken = true
				};

				var token = await this.server.ConnectAsync(serverAddress, 10000, null, loginInfo);

				this.storageService.Token = token.Token;
				this.storageService.ServerAddress = serverAddress;

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

		public async Task ConnectToken(string serverAddress, string token)
		{
			var loginInfo = new Lers.Networking.TokenAuthenticationInfo(token);

			await this.server.ConnectAsync(serverAddress, 10000, null, loginInfo);
		}


		private (string, ushort) SplitAddressPort()
		{
			throw new NotImplementedException();
		}
	}
}
