using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lers.Core;
using Lers.Utils;
using Xamarin.Forms;

namespace LersMobile.Core
{
    /// <summary>
    /// Параметры точки учёта, используемые для вывода на экран.
    /// </summary>
    public class MeasurePointView
    {
        public MeasurePoint MeasurePoint { get; private set; }

        public string Title => this.MeasurePoint.Title;

		/// <summary>
		/// Полное наименование точки учёта, включая наименование объекта.
		/// </summary>
		public string FullTitle => this.MeasurePoint.FullTitle;

		/// <summary>
		/// Связанное с точкой учёта оборудование.
		/// </summary>
		public string Device => this.MeasurePoint.Device?.ToString();

        /// <summary>
        /// Источник изображения с состоянием точки учёта.
        /// </summary>
        public string StateImageSource
        {
            get
            {
                switch (this.MeasurePoint.State)
                {
                    case MeasurePointState.Normal: return "State_Normal.png";
                    case MeasurePointState.Error: return "State_Error.png";
                    case MeasurePointState.None: return "State_Unknown.png";
                    case MeasurePointState.Warning: return "State_Warning.png";
                    default:
                        throw new NotSupportedException("Неизвестное состояние " + this.MeasurePoint.State);
                }
            }
        }

        /// <summary>
        /// Изображение типа системы.
        /// </summary>
        public string SystemTypeImageSource => ResourceHelper.GetSystemTypeImage(this.MeasurePoint.SystemType);


        public MeasurePointView(MeasurePoint measurePoint)
        {
            this.MeasurePoint = measurePoint ?? throw new ArgumentNullException(nameof(measurePoint));
        }

		/// <summary>
		/// Загружает требуемые для отображения данные по точке учёта.
		/// </summary>
		/// <returns></returns>
		public async Task LoadData()
		{
			await App.Core.EnsureConnected();

			var requiredFlags = MeasurePointInfoFlags.Equipment;

			if (!this.MeasurePoint.AvailableInfo.HasFlag(requiredFlags))
			{
				await this.MeasurePoint.RefreshAsync(requiredFlags);
			}
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

			var pollSessionId = await MeasurePoint.PollCurrentAsync(new MeasurePointPollCurrentOptions
			{
				StartMode = Lers.Common.PollManualStartMode.Force
			});

			this.tasks[pollSessionId] = tcs;

			using (var registration = cancellationToken.Register(() => tcs.TrySetCanceled()))
			{
				await tcs.Task;
			}

			server.PollSessions.PollSessionChanged -= PollSessions_PollSessionChanged;
		}

		private Dictionary<int, TaskCompletionSource<bool>> tasks = new Dictionary<int, TaskCompletionSource<bool>>();

		private void PollSessions_PollSessionChanged(object sender, Lers.Poll.PollSessionChangedEventArgs e)
		{
			if (e.PollSession == null)
			{
				return;
			}

			if (e.PollSession.EndDateTime.HasValue && e.Operation== Lers.Common.EntityOperationType.Updated)
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
						completion.TrySetException(new Exception($"Опрос завершён с ошибкой. {e.PollSession.ResultCode.GetDescription()}"));
					}					
				}
			}
		}
	}
}
