using LersMobile.Views;
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
		/// <summary>
		/// Признак того, что данные загружены
		/// </summary>
        private bool isLoaded = false;

		/// <summary>
		/// Объект учета, для которой отображается информация
		/// </summary>
        private NodeView _node;

        public NodeView Node
        {
            get => _node;
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

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="node"></param>
        public NodeCommonPropertiesPage(NodeView node)
        {
            InitializeComponent();

            this.BindingContext = this;

            this.Node = node ?? throw new ArgumentNullException(nameof(node));

			this.Title = Droid.Resources.Messages.NodeCommonPropertiesPage_Title;
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
			var detailState = (NodeStateView)e.SelectedItem;

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
				case DetailedState.CriticalIncidents:
				case DetailedState.Incidents:
					// При щелчке на НС откроем отфильтрованную страницу.
					await ShowIncidentsForNode();
					break;
			}
		}

		/// <summary>
		/// Перейти на страницу с инцидентами 
		/// </summary>
		/// <returns></returns>
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
                await DisplayAlert(Droid.Resources.Messages.Text_Error, 
					Droid.Resources.Messages.NodeCommonPropertiesPage_Error_Load_Detail + Environment.NewLine + exc.Message,
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