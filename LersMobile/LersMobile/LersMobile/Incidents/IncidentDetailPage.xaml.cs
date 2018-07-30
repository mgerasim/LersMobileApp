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

			this.Title = LersMobile.Droid.Resources.Messages.Incedent;
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
                await DisplayAlert(LersMobile.Droid.Resources.Messages.ErrorIncidentLoad, exc.Message, "OK");
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

            bool confirmed = await DisplayAlert(LersMobile.Droid.Resources.Messages.ConfirmedIncidentHeader, 
												LersMobile.Droid.Resources.Messages.ConfirmedIncidentBody, 
												LersMobile.Droid.Resources.Messages.Yes,
												LersMobile.Droid.Resources.Messages.No);

            if (!confirmed)
            {
                return;
            }

            try
            {
                await this.Incident.Close();

                // TODO: для отображение сообщений нужно использовать DependancyService, т.к. Toast.MakeText специфичен для android.
                // https://stackoverflow.com/questions/35279403/toast-equivalent-on-xamarin-forms
                // https://xamarinhelp.com/toast-notifications-xamarin-forms/
                Android.Widget.Toast.MakeText(
                    Android.App.Application.Context,
                    LersMobile.Droid.Resources.Messages.IncidentCloseSuccessed,
                    Android.Widget.ToastLength.Short)
                    .Show();
            }
            catch (Exception exc) when (exc is Lers.NoConnectionException || exc is Lers.Networking.RequestDisconnectException)
            {
            }
            catch (Exception exc)
            {
                await DisplayAlert(LersMobile.Droid.Resources.Messages.ErrotIncidentClose, exc.Message, "OK");
            }
        }
    }
}