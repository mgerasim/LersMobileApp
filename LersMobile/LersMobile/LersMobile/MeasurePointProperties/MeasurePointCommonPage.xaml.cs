using LersMobile.Services.BugReport;
using LersMobile.Views;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.MeasurePointProperties
{
	/// <summary>
	/// Страница общих свойств точки учёта.
	/// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurePointCommonPage : ContentPage
    {
		private bool isLoaded = false;

		public MeasurePointView MeasurePoint { get; private set; }

        public MeasurePointCommonPage(MeasurePointView measurePointView)
        {
			this.MeasurePoint = measurePointView ?? throw new NullReferenceException(nameof(measurePointView));

            InitializeComponent();

			this.BindingContext = this;

			this.Title = Droid.Resources.Messages.MeasurePointCommonPage_Title;
        }

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isLoaded)
			{
				return;
			}

			await LoadMeasurePointData();
		}

		protected void buttonArchive_Clicked()
		{
			Navigation.PushAsync(new MeasurePointArchivePage(MeasurePoint));
		}

		private async Task LoadMeasurePointData()
		{
			this.IsBusy = true;

			try
			{
				await this.MeasurePoint.LoadData();

				OnPropertyChanged(nameof(MeasurePoint));
			}
			catch (Exception exc)
			{
				BugReportService.HandleException(
					Droid.Resources.Messages.Text_Error_Load,
					$"{Droid.Resources.Messages.IncidentDetailPage_Error_Load_Description}. {exc.Message}",
					exc);
			}
			finally
			{
				this.IsBusy = false;
			}

			this.isLoaded = true;
		}


		/// <summary>
		/// Пользователь щёлкнул на детальной информации объекта.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void OnDetailStateSelected(object sender, SelectedItemChangedEventArgs e)
		{
			try
			{
				var listView = (ListView)sender;

				// Получаем детальную информацию
				var detailState = (MeasurePointStateView)e.SelectedItem;

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
						await ShowIncidentsForMeasurePoint();
						break;
				}
			}
			catch (Exception exc)
			{
				BugReportService.HandleException(Droid.Resources.Messages.Text_Error_Load, exc.Message, exc);
			}
		}

		private Task ShowIncidentsForMeasurePoint()
		{
			var incidentListPage = new Incidents.IncidentListPage(Incidents.PageMode.ObjectActive)
			{
				IncidentFilter = this.MeasurePoint.MeasurePoint
			};

			return this.Navigation.PushAsync(incidentListPage);
		}
	}
}