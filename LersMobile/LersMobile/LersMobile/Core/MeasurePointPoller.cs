using Lers.Common;
using Lers.Core;
using Lers.Serialization.Extensions;
using Lers.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LersMobile.Core
{
	/// <summary>
	/// Содержит методы для опроса данных по точке учёта.
	/// </summary>
	class MeasurePointPoller : IDisposable
    {
		private readonly MeasurePoint measurePoint;
		private readonly MobileCore core;

		private Dictionary<int, TaskCompletionSource<bool>> tasks = new Dictionary<int, TaskCompletionSource<bool>>();

		/// <summary>
		/// Вызывается при добавлении новой записи в журнал запущенного опроса.
		/// </summary>
		public event EventHandler<PollLogEventArgs> PollLog;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="core"></param>
		/// <param name="measurePoint"></param>
		public MeasurePointPoller(MobileCore core, MeasurePoint measurePoint)
		{
			this.core = core ?? throw new ArgumentNullException(nameof(core));
			this.measurePoint = measurePoint ?? throw new ArgumentNullException(nameof(measurePoint));
		}

		public void Dispose()
		{
			this.core.Server.RemoveNotification(PollLogMessageEventHandler);
		}

		/// <summary>
		/// Опрашивает текущие данные. Функция завершается после окончания опроса.
		/// </summary>
		/// <param name="cancellationToken">Токен, используемый для отмены ожидания.</param>
		/// <returns></returns>
		public async Task PollCurrent(CancellationToken cancellationToken)
		{
			var core = App.Core;

			await core.EnsureConnected();

			var server = core.Server;

			server.PollSessions.PollSessionChanged += PollSessions_PollSessionChanged;
			
			var tcs = new TaskCompletionSource<bool>();

			var pollSessionId = await this.measurePoint.PollCurrentAsync(new MeasurePointPollCurrentOptions
			{
				StartMode = PollManualStartMode.Force
			});

			SubscribePollMessage(pollSessionId);

			this.tasks[pollSessionId] = tcs;

			using (var registration = cancellationToken.Register(() => tcs.TrySetCanceled()))
			{
				await tcs.Task;
			}

			server.PollSessions.PollSessionChanged -= PollSessions_PollSessionChanged;

			this.core.Server.RemoveNotification(PollLogMessageEventHandler);
		}


		private void SubscribePollMessage(int pollSessionId)
		{
			this.core.Server.AddNotification(Lers.Interop.Operation.NOTIFY_POLL_LOG_MESSAGE, Lers.Interop.EntityType.PollSession, pollSessionId, true, PollLogMessageEventHandler, null);
		}		

		private void PollSessions_PollSessionChanged(object sender, Lers.Poll.PollSessionChangedEventArgs e)
		{
			if (e.PollSession == null)
			{
				return;
			}

			if (e.PollSession.EndDateTime.HasValue && e.Operation == Lers.Common.EntityOperationType.Updated)
			{
				// Сессия опроса завершена.

				if (tasks.TryGetValue(e.PollSessionId, out TaskCompletionSource<bool> completion))
				{
					tasks.Remove(e.PollSessionId);

					if (e.PollSession.ResultCode == Lers.Poll.PollError.None || e.PollSession.ResultCode == Lers.Poll.PollError.DEV_NOT_ALL_DATA_READ)
					{
						completion.TrySetResult(true);
					}
					else
					{
						completion.TrySetException(new Exception($"{LersMobile.Droid.Resources.Messages.ErrorPollSession}. {e.PollSession.ResultCode.GetDescription()}"));
					}
				}
			}
		}

		private void PollLogMessageEventHandler(PropertyBag notifyData, object userState)
		{			
			var message = notifyData.GetParameter<Lers.Models.PollSessionLogMessage>();

			PollLog?.Invoke(this, new PollLogEventArgs
			{
				Message = message.Message
			});
		}
	}
}
