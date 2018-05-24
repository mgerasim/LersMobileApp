using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Lers.Core;
using System.Collections.ObjectModel;

namespace LersMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NodePropertyPage : ContentPage
	{
		private bool isLoaded = false;

		private Core.NodeView _node;

		public Core.NodeView Node
		{
			get { return _node; }
			private set
			{
				_node = value;
				OnPropertyChanged(nameof(Node));
			}
		}

		public ObservableCollection<Core.NodeStateView> NodeState { get; private set; } = new ObservableCollection<Core.NodeStateView>();

		public NodePropertyPage(Core.NodeView node)
		{
			InitializeComponent();

			this.BindingContext = this;

			this.Node = node ?? throw new ArgumentNullException(nameof(node));
		}

		
		/// <summary>
		/// Вызывается при отображении страницы на экране.
		/// </summary>
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isLoaded)
			{
				// Данные уже загружались.
				return;
			}

			this.IsBusy = true;

			var requiredFlags = NodeInfoFlags.Serviceman | NodeInfoFlags.Customer;

			if (!this.Node.Node.AvailableInfo.HasFlag(requiredFlags))
			{
				// Нужные данные ещё не загружены

				await this.Node.Node.RefreshAsync(requiredFlags);

				// Обновляем свойства объекта.

				OnPropertyChanged(nameof(Node));
			}

			if (this.Node.Node.State != Lers.Core.NodeState.None)
			{
				// Если состояние объекта отличается от нормального,
				// загружаем диагностическую карточку.
				await LoadDiagnostics();
			}

			this.isLoaded = true;

			this.IsBusy = false;
		}

		/// <summary>
		/// Загружает диагностическую информацию по объекту.
		/// </summary>
		/// <returns></returns>
		private async Task LoadDiagnostics()
		{
			var node = this.Node.Node;

			var state = await node.GetDetailedState();

			if (state.CriticalIncidentCount > 0)
			{
				this.NodeState.Add(new Core.NodeStateView { Text = $"Критических НС: {state.CriticalIncidentCount}" });
			}

			if (state.WarningIncidentCount > 0)
			{
				this.NodeState.Add(new Core.NodeStateView { Text = $"Нештатных ситуаций: {state.WarningIncidentCount}" });
			}

			if (state.LastDataOverdue > 0)
			{
				this.NodeState.Add(new Core.NodeStateView { Text = $"Данные отсутствуют: {state.LastDataOverdue} дн." });
			}

			if (state.OverdueJobCount > 0)
			{
				this.NodeState.Add(new Core.NodeStateView { Text = $"Просрочено работ: {state.OverdueJobCount}" });
			}

			if (state.DaysToAdmissionDeadline.HasValue)
			{
				this.NodeState.Add(new Core.NodeStateView
				{
					Text = $"Допуск '{state.AdmissionMeasurePoint.Title}' заканчивается через: {state.DaysToAdmissionDeadline} дн."
				});
			}

			if (state.AdmissionDateOverdue.HasValue)
			{
				this.NodeState.Add(new Core.NodeStateView
				{
					Text = $"Допуск '{state.AdmissionMeasurePoint.Title}' просрочен на: {state.AdmissionDateOverdue} дн."
				});
			}
		}
	}
}