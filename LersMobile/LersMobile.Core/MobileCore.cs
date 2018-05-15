using Lers;
using System;
using System.Threading.Tasks;

namespace LersMobile.Core
{
    public class MobileCore
    {
		private LersServer server;

        public string Token { get; set; }

		public event EventHandler OnTokenReceived;

		public MobileCore()
		{
			this.server = new LersServer("Lers android");

			this.server.VersionMismatch += (sender, e) => e.Ignore = true;
		}

		public async Task Connect(string serverAddress, string login, string password)
		{
			var token = await this.server.ConnectAsync(serverAddress, 10000, null,
				new Lers.Networking.BasicAuthenticationInfo(login, Lers.Networking.SecureStringHelper.ConvertToSecureString(password)));

			OnTokenReceived?.Invoke(this, EventArgs.Empty);
		}

		/*public async Task ConnectToken()
		{
			var token = await this.server.ConnectAsync(serverAddress, 10000, null,
			new Lers.Networking.BasicAuthenticationInfo(login, Lers.Networking.SecureStringHelper.ConvertToSecureString(password)));
		}*/


		public Task<Notification[]> GetNotifications()
		{
			return this.server.Notifications.GetListAsync();
		}
	}
}
