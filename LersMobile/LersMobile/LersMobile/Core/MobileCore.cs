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
		private readonly IAppDataStorage storageService;

		public LersServer Server { get; }

		/// <summary>
		/// Вызывается в случае если требуется вернуть пользователя на экран входа.
		/// </summary>
		public event EventHandler LoginRequired;

		public MobileCore()
		{
			this.Server = new LersServer("Lers android");

			this.Server.VersionMismatch += (sender, e) => e.Ignore = true;

			this.storageService = DependencyService.Get<IAppDataStorage>();
		}

		/// <summary>
		/// Подключается к серверу с использованием логина и пароля.
		/// </summary>
		/// <param name="serverAddress"></param>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task Connect(string serverAddress, string login, string password)
		{
			try
			{
				var securePassword = Lers.Networking.SecureStringHelper.ConvertToSecureString(password);

				var loginInfo = new Lers.Networking.BasicAuthenticationInfo(login, securePassword)
				{
					GetSessionRestoreToken = true
				};

				var token = await this.Server.ConnectAsync(serverAddress, 10000, null, loginInfo, CancellationToken.None);

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

		/// <summary>
		/// Подключается к серверу с использованием токена аутентификации.
		/// </summary>
		/// <param name="serverAddress"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task ConnectToken(string serverAddress, string token)
		{
			var loginInfo = new Lers.Networking.TokenAuthenticationInfo(token);

			try
			{
				await this.Server.ConnectAsync(serverAddress, 10000, loginInfo, CancellationToken.None);
			}
			catch (Lers.Networking.AuthorizationFailedException)
			{
				// Произошла ошибка аутентификации. Нужно очистить токен и сообщить что требуется логин.

				ClearStoredToken();

				LoginRequired?.Invoke(this, EventArgs.Empty);
			}
		}


		public async Task<NodeDetail[]> GetNodeDetail(int? nodeGroupId)
		{
			await EnsureConnected();

			var getNodesTask = nodeGroupId.HasValue
				? this.Server.Nodes.GetListAsync(nodeGroupId.Value)
				: this.Server.Nodes.GetListAsync();

			var nodes = await getNodesTask;

			return nodes.Select(x => new NodeDetail(x)).ToArray();
		}

		public async Task<NotificationDetail[]> GetNotifications()
		{
			await EnsureConnected();

			// Уведомления запрашиваем только за последние три месяца.
			// Их может быть много, а толку от старых уведомлений нет.

			var endDate = DateTime.Now.AddDays(1);
			var startDate = endDate.AddMonths(-3);

			var list = await this.Server.Notifications.GetListAsync(startDate, endDate);

			return list.Select(x => new NotificationDetail(x))
				.OrderByDescending(x => x.DateTime)
				.ToArray();
		}

		public void Disconnect() => this.Server.Disconnect(10000);

		public void Logout()
		{
			if (!this.Server.IsConnected)
			{
				return;
			}

			this.Server.Disconnect(10000, true);

			ClearStoredToken();

			// Сообщаем о том что вновнь нужен логин

			LoginRequired?.Invoke(this, EventArgs.Empty);
		}

		private void ClearStoredToken()
		{
			this.storageService.Token = string.Empty;
			this.storageService.Save();
		}

		private async Task EnsureConnected()
		{
			if (!this.Server.IsConnected)
			{
				if (string.IsNullOrEmpty(this.storageService.ServerAddress)
				|| string.IsNullOrEmpty(this.storageService.Token))
				{
					throw new InvalidOperationException("Невозможно подключиться к серверу ЛЭРС УЧЁТ. Отсутствует адрес сервера или токен.");
				}

				await ConnectToken(this.storageService.ServerAddress, this.storageService.Token);
			}
		}
	}
}
