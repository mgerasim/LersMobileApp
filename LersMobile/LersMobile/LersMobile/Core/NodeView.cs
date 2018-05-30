using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Lers.Core;

namespace LersMobile.Core
{
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
        public ObservableCollection<NodeStateView> DetailedState { get; private set; } = new ObservableCollection<Core.NodeStateView>();


        public string State
		{
			get
			{
				switch (this.Node.State)
				{
					case NodeState.Error: return "Есть ошибки";
					case NodeState.None: return "Неизвестно";
					case NodeState.Normal: return "Норма";
					case NodeState.Warning: return "Есть предупреждения";
					default:
						throw new NotSupportedException("Неизвестное состояние " + this.Node.State);
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
                this.DetailedState.Add(new NodeStateView(node.State) { Text = $"Критических НС: {state.CriticalIncidentCount}" });
            }

            if (state.WarningIncidentCount > 0)
            {
                this.DetailedState.Add(new NodeStateView(node.State) { Text = $"Нештатных ситуаций: {state.WarningIncidentCount}" });
            }

            if (state.LastDataOverdue > 0)
            {
                this.DetailedState.Add(new NodeStateView(node.State) { Text = $"Данные отсутствуют: {state.LastDataOverdue} дн." });
            }

            if (state.OverdueJobCount > 0)
            {
                this.DetailedState.Add(new NodeStateView(node.State) { Text = $"Просрочено работ: {state.OverdueJobCount}" });
            }

            if (state.DaysToAdmissionDeadline.HasValue)
            {
                this.DetailedState.Add(new NodeStateView(node.State)
                {
                    Text = $"Допуск '{state.AdmissionMeasurePoint.Title}' заканчивается через: {state.DaysToAdmissionDeadline} дн."
                });
            }

            if (state.AdmissionDateOverdue.HasValue)
            {
                this.DetailedState.Add(new NodeStateView(node.State)
                {
                    Text = $"Допуск '{state.AdmissionMeasurePoint.Title}' просрочен на: {state.AdmissionDateOverdue} дн."
                });
            }

            if (state.LastDataOverdue > 0)
            {
                this.DetailedState.Add(new NodeStateView(node.State)
                {
                    Text = $"Данные не получены {state.LastDataOverdue} дн."
                });
            }
        }
    }
}
