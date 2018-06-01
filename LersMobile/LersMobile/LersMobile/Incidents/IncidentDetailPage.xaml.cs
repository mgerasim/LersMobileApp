using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.Incidents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentDetailPage : ContentPage
    {
        private Core.IncidentView incidentView;

        public Core.IncidentView Incident
        {
            get => this.incidentView;
            set
            {
                this.incidentView = value;
                OnPropertyChanged(nameof(Incident));
            }
        }

        public IncidentDetailPage(Core.IncidentView incidentView)
        {
            this.incidentView = incidentView ?? throw new ArgumentNullException(nameof(incidentView));

            InitializeComponent();

            this.BindingContext = this;
        }

        public void OnItemSelected(object sender, EventArgs e) => ((ListView)sender).SelectedItem = null;

        protected override async void OnAppearing()
        {
            this.IsBusy = true;

            try
            {
                await this.incidentView.LoadLog();

                OnPropertyChanged(nameof(Incident));
            }
            catch (Exception exc) when (exc is TimeoutException || exc is Lers.LersException)
            {
                await DisplayAlert("Ошибка загрузки НС", exc.Message, "OK");
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