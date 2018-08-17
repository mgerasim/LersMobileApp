using Lers;
using System;

namespace LersMobile.Core
{
    /// <summary>
    /// Вспомогательные функции для авторизации на сервере
    /// </summary>
    public static class LoginUtils
    {
        /// <summary>
        /// Порт по умолчанию сервера ЛЭРС Учёт
        /// </summary>
        public const int DefaultPort = 10000;

        /// <summary>
        /// Формирует объект для подключения Uri.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="AcceptSsl"></param>
        /// <returns>Uri</returns>
        public static Uri BuildConnectionUri(string host, int port, bool acceptSsl)
        {
            var connectionUrl = string.Empty;

            var schemaName = string.Empty;

            if (acceptSsl == true)
            {
                schemaName = Lers.LersScheme.Secure;
            }
            else
            {
                schemaName = Lers.LersScheme.Plain;
            }

            var uriBuilder = new UriBuilder(schemaName, host, port);

            return uriBuilder.Uri;
        }

        /// <summary>
        /// Формирует объект для подключения Uri.
        /// </summary>
        /// <param name="connectionUrl"></param>
        /// <param name="acceptSsl"></param>
        /// <returns>Uri</returns>
        public static Uri BuildConnectionUri(string connectionUrl, bool acceptSsl)
        {
            if (!connectionUrl.StartsWith(LersScheme.Plain))
            {
                connectionUrl = GetSchema(acceptSsl) + connectionUrl;
            }

            var uriBuilder = new UriBuilder(connectionUrl);

            var uri = uriBuilder.Uri;


            var Port = uri.Port;

            if (uri.IsDefaultPort)
            {
                Port = DefaultPort;
            }
            
            return BuildConnectionUri(uri.Host, Port, acceptSsl);
        }

        private static string GetSchema(bool acceptSsl)
        {
            var schema = string.Empty;
            if (acceptSsl)
            {
                schema = LersScheme.Secure;
            }
            else
            {
                schema = LersScheme.Plain;
            }
            schema += "://";

            return schema;
        }
    }
}
