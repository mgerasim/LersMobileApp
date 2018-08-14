﻿using Lers;
using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LersMobile.Core
{
    public class MobileCore
    {
		public LersServer Server { get; }

		/// <summary>
		/// Вызывается в случае если требуется вернуть пользователя на экран входа.
		/// </summary>
		public event EventHandler LoginRequired;

		public MobileCore()
		{
			this.Server = new LersServer("Lers android");

			this.Server.VersionMismatch += (sender, e) => e.Ignore = true;
		}

        /// <summary>
        /// Подключается к серверу с использованием логина и пароля.
        /// </summary>
        /// <param name="connectionUrl"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="acceptSsl"></param>
        /// <returns></returns>
        public async Task Connect(string connectionUrl, string login, string password, bool acceptSsl = false)
		{
			try
			{
				var securePassword = Lers.Networking.SecureStringHelper.ConvertToSecureString(password);

				var loginInfo = new Lers.Networking.BasicAuthenticationInfo(login, securePassword)
				{
					GetSessionRestoreToken = true
				};
                
                var uri = LoginUtils.BuildConnectionUri(connectionUrl, acceptSsl);
                                                
				var token = await this.Server.ConnectAsync(uri, null, loginInfo, CancellationToken.None);
                
				AppDataStorage.Token = token.Token;
				AppDataStorage.Host = uri.Host;
                AppDataStorage.Port = uri.Port;
                AppDataStorage.AcceptSsl = acceptSsl;
                AppDataStorage.Login = login;
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
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="token"></param>
        /// <param name="acceptSsl"></param>
        /// <returns></returns>
        public async Task ConnectToken(string host, int port, string token, bool acceptSsl)
		{
			var loginInfo = new Lers.Networking.TokenAuthenticationInfo(token);
            
			try
			{
                var uri = LoginUtils.BuildConnectionUri(host, port, acceptSsl);
				await this.Server.ConnectAsync(uri, null, loginInfo, CancellationToken.None);
			}
			catch (Lers.Networking.AuthorizationFailedException)
			{
				// Произошла ошибка аутентификации. Нужно очистить токен и сообщить что требуется логин.

				ClearStoredToken();

				LoginRequired?.Invoke(this, EventArgs.Empty);
			}
		}


		public async Task<NodeView[]> GetNodeDetail(int? nodeGroupId)
		{
			await EnsureConnected();

			var getNodesTask = nodeGroupId.HasValue
				? this.Server.Nodes.GetListAsync(nodeGroupId.Value)
				: this.Server.Nodes.GetListAsync();

			var nodes = await getNodesTask;

			return nodes.Select(x => new NodeView(x)).ToArray();
		}

		/// <summary>
		/// Запрос уведомлений пользователя.
		/// </summary>
		/// <returns></returns>
		public async Task<NotificationView[]> GetNotifications()
		{
			await EnsureConnected();

			// Уведомления запрашиваем только за последние три месяца.
			// Их может быть много, а толку от старых уведомлений нет.

			var endDate = DateTime.Now.AddDays(1);
			var startDate = endDate.AddMonths(-3);

			var list = await this.Server.Notifications.GetListAsync(startDate, endDate);

			return list
				.OrderByDescending(x => x.DateTime)
				.Select(x => new NotificationView(x))
				.ToArray();
		}

        public async Task<DayIncidentList[]> GetNewIncidents(int? nodeGroupId)
        {
            await EnsureConnected();

            var incidents = await this.Server.Incidents.GetListNewAsync(nodeGroupId);

            return GroupIncidentsByStartDate(incidents);
        }

        public async Task<DayIncidentList[]> GetIncidents(DateTime startDate, DateTime endDate, int? nodeGroupId)
        {
            await EnsureConnected();

            var incidents = await this.Server.Incidents.GetListAsync(startDate, endDate, nodeGroupId);

            return GroupIncidentsByStartDate(incidents);
        }

		/// <summary>
		/// Возвращает активные НС для объекта.
		/// </summary>
		/// <param name="incidentContainer"></param>
		/// <returns></returns>
		public async Task<DayIncidentList[]> GetActiveIncidents(Lers.Diag.IIncidentContainer incidentContainer)
		{
			await EnsureConnected();

			var incidents = await incidentContainer.GetActiveIncidents();

			return GroupIncidentsByStartDate(incidents);
		}

        private static DayIncidentList[] GroupIncidentsByStartDate(Lers.Diag.Incident[] incidents)
        {
            var incidentGroups = incidents
                            .OrderByDescending(x => x.EndDateTime)
                            .GroupBy(x => x.EndDateTime.Date);

            var result = new List<DayIncidentList>();

            foreach (var dayList in incidentGroups)
            {
                var list = new DayIncidentList(dayList.Key);

                foreach (var incident in dayList)
                {
                    list.Add(new IncidentView(incident));
                }

                result.Add(list);
            }

            return result.ToArray();
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
			AppDataStorage.Token = string.Empty;
		}

		public async Task EnsureConnected()
		{
			if (!this.Server.IsConnected)
			{
				if (string.IsNullOrEmpty(AppDataStorage.Host)
				|| string.IsNullOrEmpty(AppDataStorage.Token))
				{
					throw new InvalidOperationException(Droid.Resources.Messages.MobileCore_Connect_Failed);
				}

				await ConnectToken(AppDataStorage.Host, AppDataStorage.Port, AppDataStorage.Token, AppDataStorage.AcceptSsl);
			}
		}
	}
}
