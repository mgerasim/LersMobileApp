using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
	/// <summary>
	/// Аргументы события о добавлении новой записи в журнал опроса.
	/// </summary>
    public class PollLogEventArgs : EventArgs
    {
		/// <summary>
		/// Сообщение в журнале опроса.
		/// </summary>
		public string Message { get; internal set; }
    }
}
