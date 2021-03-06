﻿using Lers.Core;
using LersMobile.Services.BugReport;
using LersMobile.Services.Resource;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LersMobile.Views
{
    /// <summary>
    /// Используется для отображения на экране параметров объекта учёта.
    /// </summary>
    public class NodeView
    {
        public Node Node { get; private set; }

        public string Address => this.Node.Address;

        public string Title => this.Node.Title;

		public string ImageSource => ResourceService.NodeImageSource(Node);

		public string ServicemanName => this.Node.Serviceman?.DisplayName;

		public string CustomerTitle => this.Node.Customer?.Title;

        /// <summary>
        /// Указывает что по объекту есть дополнительная диагностическая информация.
        /// </summary>
        public bool HasDetailedState => this.Node.State != NodeState.Normal;

        /// <summary>
        /// Список точек учёта в объекте.
        /// </summary>
        public ObservableCollection<MeasurePointView> MeasurePoints { get; private set; } = new ObservableCollection<MeasurePointView>();

        /// <summary>
        /// Список детальной информации о состоянии объекта.
        /// </summary>
        public ObservableCollection<NodeStateView> DetailedState { get; private set; } = new ObservableCollection<NodeStateView>();

		/// <summary>
		/// Описание состояния объекта учёта
		/// </summary>
        public string State => ResourceService.NodeStateDesc(Node.State);


		internal NodeView(Node node)
		{
			this.Node = node ?? throw new ArgumentNullException(nameof(node));
		}

        /// <summary>
        /// Загружает детальную информацию для отображения свойств объекта учёта.
        /// </summary>
        /// <returns></returns>
        public async Task LoadDetail()
        {
			var requiredFlags = NodeInfoFlags.Serviceman | NodeInfoFlags.Customer | NodeInfoFlags.Systems;

			if (!this.Node.AvailableInfo.HasFlag(requiredFlags))
			{
				// Нужные данные ещё не загружены

				await this.Node.RefreshAsync(requiredFlags);
			}

			// Заполняем список точек учёта.

			if (this.MeasurePoints.Count == 0)
			{
				foreach (var measurePoint in this.Node.Systems.GetAllMeasurePoints())
				{
					this.MeasurePoints.Add(new MeasurePointView(measurePoint));
				}
			}

			if (this.Node.State != NodeState.Normal)
			{
				// Если состояние объекта отличается от нормального,
				// загружаем диагностическую карточку.

				await LoadDiagnostics();
			}
        }


        /// <summary>
		/// Загружает диагностическую информацию по объекту.
		/// </summary>
		/// <returns></returns>
		private async Task LoadDiagnostics()
        {
			var node = this.Node;

			var state = await node.GetDetailedState();

			this.DetailedState.Clear();

			if (state.CriticalIncidentCount > 0)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Error, Views.DetailedState.CriticalIncidents)
				{
					Text = String.Format(Droid.Resources.Messages.NodeView_CriticalIncident_Count, state.CriticalIncidentCount)
				});
			}

			if (state.WarningIncidentCount > 0)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Warning, Views.DetailedState.Incidents)
				{
					Text = String.Format(Droid.Resources.Messages.NodeView_Warning_Incident_Count, state.WarningIncidentCount)
				});
			}

			if (state.LastDataOverdue > 0)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Warning) { Text = String.Format(Droid.Resources.Messages.NodeView_Overdue_Data, state.LastDataOverdue) });
			}

			if (state.OverdueJobCount > 0)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Error) { Text = String.Format(Droid.Resources.Messages.NodeView_OverdueJobCount, state.OverdueJobCount) });
			}

			if (state.DaysToAdmissionDeadline.HasValue)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Warning)
				{
					Text = String.Format(Droid.Resources.Messages.NodeView_Admission_Deadline, state.AdmissionMeasurePoint.Title, state.DaysToAdmissionDeadline)
				});
			}

			if (state.AdmissionDateOverdue.HasValue)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Error)
				{
					Text = String.Format(Droid.Resources.Messages.NodeView_Admission_Overdue, state.AdmissionMeasurePoint.Title, state.AdmissionDateOverdue)
				});
			}

			if (state.DueEquipmentCalibrationCount > 0)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Warning)
				{
					Text = String.Format(Droid.Resources.Messages.NodeView_DueEquipmentCalibrationCount, state.DueEquipmentCalibrationCount)
				});
			}

			if (state.OverdueEquipmentCalibrationCount > 0)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Error)
				{
					Text = String.Format(Droid.Resources.Messages.NodeView_OverdueEquipmentCalibrationCount, state.OverdueEquipmentCalibrationCount)
				});
			}
		}
    }
}
