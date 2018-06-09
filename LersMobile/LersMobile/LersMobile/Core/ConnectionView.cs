﻿using Lers.Poll;
using System;

namespace LersMobile.Core
{
	/// <summary>
	/// Параметры подключения, используемые для вывода на экран.
	/// </summary>
	public class ConnectionView
    {
		public PollConnection Connection { get; private set; }

		/// <summary>
		/// Наименование подключения.
		/// </summary>
		public string Title => this.Connection.Title;

		/// <summary>
		/// Изображение с типом канала.
		/// </summary>
		public string TypeImage => ResourceHelper.GetConnectionTypeImage(this.Connection.CommLinkType);

		public ConnectionView(PollConnection pollConnection)
		{
			this.Connection = pollConnection ?? throw new ArgumentNullException(nameof(pollConnection));
		}
    }
}
