using Lers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LersMobile.Core
{
    public class MobileCore
    {
		private LersServer server;

		private readonly IAppDataStorage storageService;

		public LersServer Server => this.server;

		/// <summary>
		/// Вызывается в случае если требуется вернуть пользователя на экран входа.
		/// </summary>
		public event EventHandler LoginRequired;

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

				var token = await this.server.ConnectAsync(serverAddress, 10000, null, loginInfo, CancellationToken.None);

				this.storageService.Token = token.Token;
				this.storageService.ServerAddress = serverAddress;

				this.storageService.Save();
			}
			catch (Lers.Networking.AuthorizationFailedException)
			{
				// Произошла ошибка аутентификации. Нужно очистить токен и выдать ошибку.

				ClearStoredToken();

				throw;
			}
		}

		public async Task ConnectToken(string serverAddress, string token)
		{
			var loginInfo = new Lers.Networking.TokenAuthenticationInfo(token);

			try
			{
				await this.server.ConnectAsync(serverAddress, 10000, loginInfo, CancellationToken.None);
			}
			catch (Lers.Networking.AuthorizationFailedException)
			{
				// Произошла ошибка аутентификации. Нужно очистить токен и сообщить что требуется логин.

				ClearStoredToken();

				LoginRequired?.Invoke(this, EventArgs.Empty);
			}
		}

		public async Task EnsureConnected()
		{
			if (!this.server.IsConnected)
			{
				if (string.IsNullOrEmpty(this.storageService.ServerAddress)
				|| string.IsNullOrEmpty(this.storageService.Token))
				{
					throw new InvalidOperationException("Невозможно подключиться к серверу ЛЭРС УЧЁТ. Отсутствует адрес сервера или токен.");
				}

				await ConnectToken(this.storageService.ServerAddress, this.storageService.Token);
			}
		}

		public async Task<NodeDetail[]> GetNodeDetail(int? nodeGroupId)
		{
			var getNodesTask = nodeGroupId.HasValue
				? this.server.Nodes.GetListAsync(nodeGroupId.Value)
				: this.server.Nodes.GetListAsync();

			var nodes = await getNodesTask;

			return nodes.Select(x => new NodeDetail(x)).ToArray();
		}

		public async Task<NotificationDetail[]> GetNotifications()
		{
			var list = await this.server.Notifications.GetListAsync();

			return list.Select(x => new NotificationDetail(x)).ToArray();
		}

		public void Disconnect() => this.server.Disconnect(10000);

		public void Logout()
		{
			this.server.Disconnect(10000, true);

			ClearStoredToken();

			// Сообщаем о том что вновнь нужен логин

			LoginRequired?.Invoke(this, EventArgs.Empty);
		}

		private void ClearStoredToken()
		{
			this.storageService.Token = string.Empty;
			this.storageService.Save();
		}
	}
}
