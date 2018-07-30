using System;
using System.Threading;
using System.Threading.Tasks;
using Lers.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.MeasurePointProperties
{
	/// <summary>
	/// Страница отображает последние данные по точке учёта и предоставляет возможность запустить опрос текущих.
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurePointDataPage : ContentPage
    {
		private bool isLoaded = false;

        private Core.MeasurePointView _measurePoint;

        public Core.MeasurePointView MeasurePoint
        {
            get { return _measurePoint; }
            private set
            {
                _measurePoint = value;
                OnPropertyChanged(nameof(MeasurePoint));
            }

        }

		private string _loadingText;

		/// <summary>
		/// Текст, отображаемый в индикаторе загрузки.
		/// </summary>
		public string LoadingText
		{
			get => _loadingText;
			set
			{
				_loadingText = value;
				OnPropertyChanged(nameof(LoadingText));
			}
		}


		private Lers.Data.DataRecord _lastDataRecord;

		public Lers.Data.DataRecord LastDataRecord
		{
			get => _lastDataRecord;
			set
			{
				_lastDataRecord = value;
				OnPropertyChanged(nameof(LastDataRecord));
			}
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="measurePoint"></param>
		public MeasurePointDataPage(Core.MeasurePointView measurePoint)
        {
            InitializeComponent();

            this.BindingContext = this;

            this.MeasurePoint = measurePoint ?? throw new ArgumentNullException(nameof(measurePoint));

			this.Title = Droid.Resources.Messages.MeasurePointDataPage_Title;
        }

		/// <summary>
		/// Пользователь нажал на кнопку "Опросить текущие"
		/// </summary>
		public async void OnPollCurrentClicked()
		{
			this.pollCurrentButton.IsEnabled = false;
			this.IsBusy = true;
			this.LoadingText = Droid.Resources.Messages.MeasurePointDataPage_Poll_Current_Loading;

			const int timeoutMinutes = 2;

			try
			{
				using (var poller = new Core.MeasurePointPoller(App.Core, this.MeasurePoint.MeasurePoint))
				{
					using (var cts = new CancellationTokenSource())
					{
						// Две минуты на окончание опроса.
						cts.CancelAfter(timeoutMinutes * 60 * 1000);

						// Подпишемся на событие добавления новой записи в журнал опроса.
						poller.PollLog += Poller_PollLog;

						await poller.PollCurrent(cts.Token);
					}
				}

				await LoadLastData();
			}
			catch (OperationCanceledException)
			{
				await DisplayAlert(Droid.Resources.Messages.MeasurePointDataPage_Poll_Current, String.Format(Droid.Resources.Messages.MeasurePointDataPage_PollCurrent_Timeout_Error, timeoutMinutes), "OK");
			}
			catch (Exception exc)
			{
				await DisplayAlert(Droid.Resources.Messages.MeasurePointDataPage_Error_Poll_Current, exc.Message, "OK");
			}
			finally
			{
				this.pollCurrentButton.IsEnabled = true;
				this.IsBusy = false;
			}
		}

		private void Poller_PollLog(object sender, Core.PollLogEventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				this.LoadingText = e.Message;
			});
		}

		/// <summary>
		/// Вызывается при отображении страницы.
		/// </summary>
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isLoaded)
			{
				return;
			}

			await LoadLastData();

			this.isLoaded = true;
		}

		/// <summary>
		/// Загружает последние данные по точке учёта.
		/// </summary>
		/// <returns></returns>
		private async Task LoadLastData()
		{
			// Обновляем последние текущие данные.

			this.LoadingText = Droid.Resources.Messages.MeasurePointDataPage_Loading;
			this.IsBusy = true;

			try
			{
				await App.Core.EnsureConnected();

				var measurePoint = this.MeasurePoint.MeasurePoint;

				var lastConsumption = await measurePoint.Data.GetLastConsumptionAsync();
				
				this.LastDataRecord = lastConsumption;				
			}
			catch (Exception exc)
			{
				await DisplayAlert(Droid.Resources.Messages.MeasurePointDataPage_ErrorData_Loaded, exc.Message, "OK");
			}
			finally
			{
				this.IsBusy = false;
			}
		}
	}
}