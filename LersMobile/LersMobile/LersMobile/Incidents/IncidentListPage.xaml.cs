using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Lers.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LersMobile.Incidents
{
    /// <summary>
    /// Отображает список нештатных ситуаций.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentListPage : ContentPage
    {
        private readonly PageMode pageMode;

        private readonly Core.MobileCore core;

        private int? SelectedNodeGroupId => null;

        private bool isLoaded = false;

        public bool ShowDateRangeControls => this.pageMode == PageMode.Interval;

        public ObservableCollection<Core.DayIncidentList> IncidentList { get; private set; } = new ObservableCollection<Core.DayIncidentList>();

		/// <summary>
		/// Объект, по которому запрашиваются НС. Используется в режиме <see cref="PageMode.ObjectActive"/>
		/// </summary>
		public Lers.Diag.IIncidentContainer IncidentFilter { get; set; }

        /// <summary>
        /// Команда, вызываемая из списка для обновления данных.
        /// </summary>
        public ICommand RefreshListView => new Command(async () => await LoadIncidents());

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pageMode"></param>
        public IncidentListPage(PageMode pageMode)
        {			
            this.pageMode = pageMode;

            InitializeComponent();

            this.Title = pageMode.GetDescription();

            this.core = App.Core;

            this.BindingContext = this;

            this.startDatePicker.Date = DateTime.Today.AddDays(-7);
            this.endDatePicker.Date = DateTime.Today;
        }

        /// <summary>
        /// Пользователь нажал кнопку "Обновить".
        /// </summary>
        public async void OnRefresh() => await LoadIncidents();


        /// <summary>
        /// Пользователь выбрал нештатную ситуацию.
        /// </summary>
        public async void OnIncidentSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var listView = (ListView)sender;

            var incidentView = (Core.IncidentView)args.SelectedItem;

            if (incidentView == null)
            {
                return;
            }

            listView.SelectedItem = null;

            await this.Navigation.PushAsync(new IncidentDetailPage(incidentView));
        }


        /// <summary>
        /// Вызывается при отображении страницы.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!this.isLoaded)
            {
                await LoadIncidents();
            }

            this.isLoaded = true;
        }


        /// <summary>
        /// Загружает список нештатных ситуаций.
        /// </summary>
        /// <returns></returns>
        private async Task LoadIncidents()
        {
            string message = CheckUserInput();

            if (!string.IsNullOrEmpty(message))
            {
                await DisplayAlert(LersMobile.Droid.Resources.Messages.Error, message, "OK");
                return;
            }

            this.IsBusy = true;

            try
            {
                Task<Core.DayIncidentList[]> getTask;

                if (this.pageMode == PageMode.NewOnly)
                {
                    getTask = this.core.GetNewIncidents(this.SelectedNodeGroupId);
                }
                else if (this.pageMode == PageMode.Interval)
                {
                    getTask = this.core.GetIncidents(this.startDatePicker.Date, this.endDatePicker.Date.Date.AddDays(1), this.SelectedNodeGroupId);
                }
				else
				{
					getTask = this.core.GetActiveIncidents(this.IncidentFilter);
				}
            
                var incidents = await getTask;

                this.IncidentList.Clear();

                this.IncidentList.AddRange(incidents);
            }
            catch (Exception exc)
            {
                await DisplayAlert(LersMobile.Droid.Resources.Messages.ErrorIncidentLoad, exc.Message, "OK");
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        /// <summary>
        /// Проверяет введённые пользователем данные.
        /// </summary>
        /// <returns>Описание ошибки или пустую строку если ошибок нет.</returns>
        private string CheckUserInput()
        {
            if (this.pageMode == PageMode.NewOnly)
            {
                return string.Empty;
            }

            else if (this.endDatePicker.Date < this.startDatePicker.Date)
            {
                return LersMobile.Droid.Resources.Messages.ErrorDateStartEnd;
            }

            return string.Empty;
        }
    }
}