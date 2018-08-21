using Lers.Core;
using LersMobile.Core;
using LersMobile.Services.BugReport;
using LersMobile.Services.Resource;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LersMobile.Views
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
		public string StateImageSource => ResourceService.StateImageSource(this.MeasurePoint);

        /// <summary>
        /// Изображение типа системы.
        /// </summary>
        public string SystemTypeImageSource => ResourceService.SystemTypeImage(this.MeasurePoint.SystemType);


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
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Error, Views.DetailedState.CriticalIncidents)
				{
					Text = $"{Droid.Resources.Messages.MeasurePointView_CriticalIncident_Count}: {state.CriticalIncidentCount}"
				});
			}

			if (state.WarningIncidentCount > 0)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Warning, Views.DetailedState.Incidents)
				{
					Text = String.Format(Droid.Resources.Messages.MeasurePointView_Warning_Incident_Count,
											state.WarningIncidentCount)
				});
			}

			if (state.OverdueJobCount > 0)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Error)
				{
					Text = String.Format(Droid.Resources.Messages.MeasurePointView_OverdueJobCount, state.OverdueJobCount)
				});
			}

			if (state.DaysToAdmissionDeadline.HasValue)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Warning)
				{
					Text = String.Format(Droid.Resources.Messages.MeasurePointView_Admission_Deadline, state.DaysToAdmissionDeadline)
				});
			}

			if (state.AdmissionDateOverdue.HasValue)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Error)
				{
					Text = String.Format(Droid.Resources.Messages.MeasurePointView_Admission_Overdue, state.AdmissionDateOverdue)
				});
			}

			if (state.LastDataOverdue > 0)
			{
				this.DetailedState.Add(new MeasurePointStateView(MeasurePointState.Warning)
				{
					Text = String.Format(Droid.Resources.Messages.MeasurePointView_Overdue_Data, state.LastDataOverdue)
				});
			}			
		}
	}
}
