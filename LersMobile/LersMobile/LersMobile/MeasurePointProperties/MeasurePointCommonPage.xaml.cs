using LersMobile.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.MeasurePointProperties
{
	/// <summary>
	/// Общие свойства точки учёта.
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
				await DisplayAlert("Ошибка загрузки", $"Не удалось загрузить информацию о точке учёта. {exc.Message}", "OK");
			}
			finally
			{
				this.IsBusy = false;
			}

			this.isLoaded = true;
		}
	}
}