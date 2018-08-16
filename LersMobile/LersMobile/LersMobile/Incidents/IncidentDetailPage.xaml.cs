using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.Incidents
{
    /// <summary>
    /// Детальная информация о нештатной ситуации.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentDetailPage : ContentPage
    {
        private bool isLoaded = false;

        private Core.IncidentView incidentView;

        /// <summary>
        /// Параметры отображаемой НС.
        /// </summary>
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

			this.Title = Droid.Resources.Messages.IncidentDetailPage_Title;
        }

        /// <summary>
        /// Вызывается при отображении страницы на экране.
        /// </summary>
        protected override async void OnAppearing()
        {
            if (this.isLoaded)
            {
                return;
            }

            this.IsBusy = true;

            try
            {
                await this.incidentView.LoadLog();

                OnPropertyChanged(nameof(Incident));

                this.isLoaded = true;
            }
            catch (Exception exc) when (exc is TimeoutException || exc is Lers.LersException)
            {
                await DisplayAlert(Droid.Resources.Messages.IncidentDetailPage_Error_Load_Incident, exc.Message, "OK");
            }
            catch (Exception exc) when (exc is Lers.NoConnectionException || exc is Lers.Networking.RequestDisconnectException)
            {
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public void OnItemSelected(object sender, EventArgs e) => ((ListView)sender).SelectedItem = null;


        /// <summary>
        /// Пользователь нажал на кнопку закрытия НС.
        /// </summary>
        public async void OnCloseIncidentClicked()
        {
            // Запрашиваем подтверждение.

            bool confirmed = await DisplayAlert(Droid.Resources.Messages.Text_Close_Incident_Short, 
												Droid.Resources.Messages.Text_Close_Incident_Full_Confirm, 
												Droid.Resources.Messages.Text_Yes,
												Droid.Resources.Messages.Text_No);

            if (!confirmed)
            {
                return;
            }

            try
            {
                await this.Incident.Close();

                DependencyService.Get<IMessage>().Show(Droid.Resources.Messages.IncidentDetailPage_IncidentCloseSuccessed);
            }
            catch (Exception exc) when (exc is Lers.NoConnectionException || exc is Lers.Networking.RequestDisconnectException)
            {
            }
            catch (Exception exc)
            {
                await DisplayAlert(Droid.Resources.Messages.IncidentDetailPage_Errot_Incident_Close, exc.Message, "OK");
            }
        }
    }
}