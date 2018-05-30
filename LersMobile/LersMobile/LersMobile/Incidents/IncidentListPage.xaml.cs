using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Lers.Utils;
using System.Collections.ObjectModel;

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

        public bool ShowDateRangeControls => this.pageMode == PageMode.Interval;

        public ObservableCollection<Core.IncidentView> IncidentList { get; private set; } = new ObservableCollection<Core.IncidentView>();


        public IncidentListPage(PageMode pageMode)
        {
            this.pageMode = pageMode;

            InitializeComponent();

            this.Title = pageMode.GetDescription();

            this.core = App.Core;

            this.BindingContext = this;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            this.IsBusy = true;

            try
            {
                await LoadIncidents();
            }
            catch (Exception exc) when (exc is TimeoutException || exc is Lers.NoConnectionException || exc is Lers.Networking.RequestDisconnectException)
            {
                await DisplayAlert("Ошибка загрузка списка НС.", exc.Message, "OK");
            }
            finally
            {
                this.IsBusy = false;
            }
        }


        private async Task LoadIncidents()
        {
            Task<Core.IncidentView[]> getTask;

            if (this.pageMode == PageMode.NewOnly)
            {
                getTask = this.core.GetNewIncidents(this.SelectedNodeGroupId);
            }
            else
            {
                getTask = this.core.GetIncidents(DateTime.Today.AddDays(-7), DateTime.Now, this.SelectedNodeGroupId);
            }
            
            var incidents = await getTask;

            this.IncidentList.Clear();

            this.IncidentList.AddRange(incidents);
        }
    }
}