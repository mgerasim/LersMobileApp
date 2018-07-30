using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.NodeProperties
{
    /// <summary>
    /// Станица, отображающая общие свойства объекта учёта.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NodeCommonPropertiesPage : ContentPage
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

        /// <summary>
        /// Указывает что по объекту учёта есть детальное состояние (диагностическая карточка).
        /// </summary>
        public bool HasDetailedState => this.Node?.HasDetailedState == true;


        public NodeCommonPropertiesPage(Core.NodeView node)
        {
            InitializeComponent();

            this.BindingContext = this;

            this.Node = node ?? throw new ArgumentNullException(nameof(node));

			this.Title = LersMobile.Droid.Resources.Messages.Total;
        }

		/// <summary>
		/// Пользователь щёлкнул на детальной информации объекта.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void OnDetailStateSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var listView = (ListView)sender;

			// Получаем детальную информацию
			var detailState = (Core.NodeStateView)e.SelectedItem;

			if (listView != null)
			{
				listView.SelectedItem = null;
			}

			if (detailState == null)
			{
				return;
			}

			// Проверим идентификатор детальной информации.

			switch (detailState.Id)
			{
				case Core.DetailedStateId.CriticalIncidents:
				case Core.DetailedStateId.Incidents:
					// При щелчке на НС откроем отфильтрованную страницу.
					await ShowIncidentsForNode();
					break;
			}
		}

		private Task ShowIncidentsForNode()
		{
			var incidentListPage = new Incidents.IncidentListPage(Incidents.PageMode.ObjectActive)
			{
				IncidentFilter = this.Node.Node
			};

			return this.Navigation.PushAsync(incidentListPage);
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

            try
            {
                await this.Node.LoadDetail();

                // Обновляем свойства объекта.

                OnPropertyChanged(nameof(Node));

                this.isLoaded = true;
            }
            catch (Exception exc) when (exc is TimeoutException || exc is Lers.LersException)
            {
                await DisplayAlert(LersMobile.Droid.Resources.Messages.Error, 
					LersMobile.Droid.Resources.Messages.ErrorLoadDetail + Environment.NewLine + exc.Message,
                    "OK");
            }
            catch (Exception exc) when (exc is Lers.NoConnectionException || exc is Lers.Networking.RequestDisconnectException)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}