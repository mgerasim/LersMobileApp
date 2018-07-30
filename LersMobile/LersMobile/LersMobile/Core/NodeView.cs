using Lers.Core;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LersMobile.Core
{
	/// <summary>
	/// Используется для отображения на экране параметров объекта учёта.
	/// </summary>
	public class NodeView
    {
		public Node Node { get; private set; }		

		public string Address => this.Node.Address;

		public string Title => this.Node.Title;

		public string ImageSource
		{
			get
			{
				switch (this.Node.State)
				{
					case NodeState.Error: return "node_red.png";
					case NodeState.Normal: return "node_green.png";
					case NodeState.Warning: return "node_orange.png";
					case NodeState.None: return "node_gray.png";

					default:
						throw new NotSupportedException(this.Node.State.ToString());
				}
			}
		}

		public string ServicemanName => this.Node.Serviceman?.Name;

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


        public string State
		{
			get
			{
				switch (this.Node.State)
				{
					case NodeState.Error: return LersMobile.Droid.Resources.Messages.NodeStateError;
					case NodeState.None: return LersMobile.Droid.Resources.Messages.NodeStateNone;
					case NodeState.Normal: return LersMobile.Droid.Resources.Messages.NodeStateNormal;
					case NodeState.Warning: return LersMobile.Droid.Resources.Messages.NodeStateWarning;
					default:
						throw new NotSupportedException($"{LersMobile.Droid.Resources.Messages.NotSupportedException} {this.Node.State}");
				}
			}
		}

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
                this.DetailedState.Add(new NodeStateView(NodeState.Error, DetailedStateId.CriticalIncidents)
				{
					Text = $"{LersMobile.Droid.Resources.Messages.CriticalIncidentCount}: {state.CriticalIncidentCount}"
				});
            }

            if (state.WarningIncidentCount > 0)
            {
                this.DetailedState.Add(new NodeStateView(NodeState.Warning, DetailedStateId.Incidents)
				{
					Text = $"{LersMobile.Droid.Resources.Messages.WarningIncidentCount}: {state.WarningIncidentCount}"
				});
            }

            if (state.LastDataOverdue > 0)
            {
                this.DetailedState.Add(new NodeStateView(NodeState.Warning) { Text = $"{LersMobile.Droid.Resources.Messages.LastDataOverdue}: {state.LastDataOverdue} " });
            }

            if (state.OverdueJobCount > 0)
            {
                this.DetailedState.Add(new NodeStateView(NodeState.Error) { Text = $"{LersMobile.Droid.Resources.Messages.OverdueJobCount}: {state.OverdueJobCount}" });
            }

            if (state.DaysToAdmissionDeadline.HasValue)
            {
				this.DetailedState.Add(new NodeStateView(NodeState.Warning)
				{
					Text = $"{state.AdmissionMeasurePoint.Title} - {LersMobile.Droid.Resources.Messages.DaysToAdmissionDeadline}: {state.DaysToAdmissionDeadline} "
                });
            }

            if (state.AdmissionDateOverdue.HasValue)
            {
                this.DetailedState.Add(new NodeStateView(NodeState.Error)
                {
                    Text = $"{state.AdmissionMeasurePoint.Title}- {LersMobile.Droid.Resources.Messages.AdmissionDateOverdue}: {state.AdmissionDateOverdue} "
                });
            }

			if (state.DueEquipmentCalibrationCount > 0)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Warning)
				{
					Text = $"{LersMobile.Droid.Resources.Messages.DueEquipmentCalibrationCount}: {state.DueEquipmentCalibrationCount} "
				});
			}

			if (state.OverdueEquipmentCalibrationCount > 0)
			{
				this.DetailedState.Add(new NodeStateView(NodeState.Error)
				{
					Text = $"{LersMobile.Droid.Resources.Messages.OverdueEquipmentCalibrationCount}: {state.OverdueEquipmentCalibrationCount} "
				});
			}
		}
    }
}
