using Lers.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LersMobile.Core
{
	/// <summary>
	/// Параметры точки учёта, используемые для вывода на экран.
	/// </summary>
	public class MeasurePointView
	{
		public MeasurePoint MeasurePoint { get; private set; }

		/// <summary>
		/// Наименование точки учёта.
		/// </summary>
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
		/// Указывает что по точке учёта включен автоопрос.
		/// </summary>
		public bool AutoPollEnabled => this.MeasurePoint.AutoPoll?.Enabled == true;

		/// <summary>
		/// Подключение, используемое для автоопроса.
		/// </summary>
		public ConnectionView AutoPollConnection { get; private set; }

		/// <summary>
		/// Отображаемые параметры точки учёта.
		/// </summary>
		public Lers.Data.DataParameter[] DisplayParameters => this.MeasurePoint.DataParameters;

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


		/// <summary>
		/// Список детальной информации о состоянии точки учёта.
		/// </summary>
		public ObservableCollection<MeasurePointStateView> DetailedState { get; private set; } = new ObservableCollection<MeasurePointStateView>();

		/// <summary>
		/// Признак, указывающий что у точки учёта есть диагностическая карточка.
		/// </summary>
		public bool HasDetailedState => this.DetailedState.Count > 0;
		

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

			var requiredFlags = MeasurePointInfoFlags.Equipment | MeasurePointInfoFlags.AutoPoll;

			if (!this.MeasurePoint.AvailableInfo.HasFlag(requiredFlags))
			{
				await this.MeasurePoint.RefreshAsync(requiredFlags);
			}

			// Загружаем диагностическую карточку точки учёта.

			if (this.MeasurePoint.State != MeasurePointState.Normal && this.DetailedState.Count == 0)
			{
				await LoadDiagnostics();
			}
			
			if (this.MeasurePoint.AutoPoll.Enabled && this.AutoPollConnection == null)
			{
				var connection = this.MeasurePoint.Device.PollSettings.Connections.First(c => c.Id == this.MeasurePoint.AutoPoll.PollConnectionId);

				this.AutoPollConnection = new ConnectionView(connection);
			}
		}


		/// <summary>
		/// Загружает диагностическую информацию по объекту.
		/// </summary>
		/// <returns></returns>
		private async Task LoadDiagnostics()
		{
			var measurePoint = this.MeasurePoint;

			var state = await measurePoint.GetDetailedState();

			this.DetailedState.Clear();

			if (state.CriticalIncidentCount > 0)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Error, DetailedStateId.CriticalIncidents)
				{
					Text = $"Критических НС: {state.CriticalIncidentCount}"
				});
			}

			if (state.WarningIncidentCount > 0)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Warning, DetailedStateId.Incidents)
				{
					Text = $"Нештатных ситуаций: {state.WarningIncidentCount}"
				});
			}

			if (state.OverdueJobCount > 0)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Error) { Text = $"Просрочено работ: {state.OverdueJobCount}" });
			}

			if (state.DaysToAdmissionDeadline.HasValue)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Warning)
				{
					Text = $"Допуск заканчивается через: {state.DaysToAdmissionDeadline} дн."
				});
			}

			if (state.AdmissionDateOverdue.HasValue)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Error)
				{
					Text = $"Допуск просрочен на: {state.AdmissionDateOverdue} дн."
				});
			}

			if (state.LastDataOverdue > 0)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Warning)
				{
					Text = $"Данные отсутствуют: {state.LastDataOverdue} дн."
				});
			}
		}
	}
}
