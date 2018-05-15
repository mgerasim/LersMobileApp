using Lers;
using System;
using System.Threading.Tasks;

namespace LersMobile.Core
{
    public class MobileCore
    {
		private LersServer server;

        public string Token { get; set; }

		public async Task Connect(string serverAddress, string login, string password)
		{
			this.server = new LersServer("Lers android");

			await this.server.ConnectAsync(serverAddress, 10000, null,
				new Lers.Networking.BasicAuthenticationInfo(login, Lers.Networking.SecureStringHelper.ConvertToSecureString(password)));
		}
	}
}
