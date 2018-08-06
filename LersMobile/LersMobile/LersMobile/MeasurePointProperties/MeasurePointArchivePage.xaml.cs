using Lers.Data;
using Lers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.MeasurePointProperties
{
	enum PeriodTypeSelected
	{
		selectDay = 0,
		selectWeek = 1,
		selectWeekTwo = 2,
		selectMonth = 3,
		selectMonthBegin = 4
	}

    enum SourceTypeSelected
    {
        selectConsumption = 0,
        selectTotals = 1
    }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MeasurePointArchivePage : ContentPage
	{

		private static DeviceDataType[] DataTypes = new DeviceDataType[]
		{
			DeviceDataType.Day, DeviceDataType.Hour, DeviceDataType.Month
		};

        private static String[] SourceTypes = new String[]
        {
            "Потребление", "Интеграторы"
        };

		private static String[] dateStringFormat = new String[]
		{
			"{0:dd.MM.yyyy}", "{0:dd.MM.yyyy HH:mm}", "{0:MM.yyyy}"
		};

		private static String[] periodRelative = new String[]
		{
			"За сутки",
			"За 7 дней",
			"За 2 недели",
			"За месяц",
			"С начала месяца"
		};
		/// <summary>
		/// Выбранный для отображения тип данных.
		/// </summary>
		private DeviceDataType SelectedDataType => DataTypes[this.dataTypePicker.SelectedIndex];
        
        /// <summary>
        /// Выбранный для отображения тип источника (потребление/интеграторы).
        /// </summary>
        private String SelectedSourceType => SourceTypes[this.sourceTypePicker.SelectedIndex];

        private readonly Core.MeasurePointView _measurePoint;

		private readonly Core.MobileCore core;

		private bool isLoaded = false;

		private String SelectedStringFormat => dateStringFormat[this.dataTypePicker.SelectedIndex];

		private String SelectedPeriod => periodRelative[this.periodRelativePicker.SelectedIndex];

		public ObservableCollection<DataRecord> Data { get; private set; } = new ObservableCollection<DataRecord>();

		private DataRecord _currentDataRecord;

		public DataRecord CurrentDataRecord
		{
			get => _currentDataRecord;
			set
			{
				_currentDataRecord = value;
				OnPropertyChanged(nameof(CurrentDataRecord));
			}
		}		
		
		public MeasurePointArchivePage(Core.MeasurePointView measurePoint)
		{
			BindingContext = this;

			InitializeComponent();

			this.core = App.Core;

			_measurePoint = measurePoint;
			
			FillDataTypes();

            FillSourceTypes();

			FillPeriodRelative();
			
			this.BindingContext = this;

			this.Title = Droid.Resources.Messages.MeasurePointArchivePage_Title;

		}

		private void UpdateDataGrid()
		{
            BuilderTableHeader();

			containerStackLayout.Children.Clear();

            if (sourceTypePicker.SelectedIndex == (int)SourceTypeSelected.selectTotals )
            {
                dataGrid.Columns[0].StringFormat = "{0:dd.MM.yyyy HH:mm}";
            }
            else
            {
                dataGrid.Columns[0].StringFormat = SelectedStringFormat;
            }
			
			containerStackLayout.Children.Add(dataGrid);
		}

		private void BuilderTableHeader()
		{
            dataGrid.Columns.Clear();

            Xamarin.Forms.DataGrid.DataGridColumn columnDateTime = new Xamarin.Forms.DataGrid.DataGridColumn();

            columnDateTime.Title = "Дата";
            columnDateTime.PropertyName = "DateTime";

            dataGrid.Columns.Add(columnDateTime);

            foreach (var param in _measurePoint.MeasurePoint.DataParameters)
			{
                if (this.sourceTypePicker.SelectedIndex == (int)SourceTypeSelected.selectTotals && !DataParameterDescriptor.Get(param).IsAdditive)
                {
                    continue;
                }

                Xamarin.Forms.DataGrid.DataGridColumn columnItem = new Xamarin.Forms.DataGrid.DataGridColumn();

				var desc = DataParameterDescriptor.Get(param);

                columnItem.Title = desc.ShortTitle;
				columnItem.PropertyName = desc.Name;
				columnItem.StringFormat = "{0:0.00}";
				
				dataGrid.Columns.Add(columnItem);
			}
		}
		
		private void FillPeriodRelative()
		{
			foreach (var period in periodRelative)
			{
				periodRelativePicker.Items.Add(period);
			}

			SetPeriodByType(PeriodTypeSelected.selectWeek);
			
			periodRelativePicker.SelectedIndex = (int)PeriodTypeSelected.selectWeek;
		}

        private void FillSourceTypes()
        {
            foreach (var sourceType in SourceTypes)
            {
                this.sourceTypePicker.Items.Add(sourceType);
            }

            this.sourceTypePicker.SelectedIndex = (int)SourceTypeSelected.selectConsumption;
        }

        private void FillDataTypes()
        {
            foreach (var dataType in DataTypes)
            {
                this.dataTypePicker.Items.Add(dataType.GetDescription());
            }

            this.dataTypePicker.SelectedIndex = 0;
        }

        /// <summary>
        /// Вызывается при отображении на экране.
        /// </summary>
        protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isLoaded)
			{
				return;
			}
			
			this.isLoaded = true;
			
			await LoadRecords();

			BuilderTableHeader();

			UpdateDataGrid();
		}
        
		private void Filter_ToolbarItem_Clicked()
		{
            if (this.sourceTypePicker.SelectedIndex == (int)SourceTypeSelected.selectTotals)
            {
                stackFilter01.IsVisible = false;
            }
            else
            {
                stackFilter01.IsVisible = !stackFilter01.IsVisible;

            }
            stackFilter02.IsVisible = !stackFilter02.IsVisible;
            stackFilter03.IsVisible = !stackFilter03.IsVisible;
            stackFilter04.IsVisible = !stackFilter04.IsVisible;
        }

		private async Task LoadRecords()
		{
			this.IsBusy = true;

			try
			{
				if (startDatePicker.Date.Year < 2000) return;

				await this.core.EnsureConnected();

                this.Data.Clear();

                switch (sourceTypePicker.SelectedIndex)
                {
                    case (int)SourceTypeSelected.selectConsumption:
                        this.Data.AddRange((await this._measurePoint.MeasurePoint.Data.GetConsumptionAsync(startDatePicker.Date, endDatePicker.Date, this.SelectedDataType)).OrderByDescending(x => x.DateTime));
                        break;
                    case (int)SourceTypeSelected.selectTotals:
                        this.Data.AddRange((await this._measurePoint.MeasurePoint.Data.GetTotalsAsync(startDatePicker.Date, endDatePicker.Date)).OrderByDescending(x => x.DateTime));
                        break;

                }
			}
			finally
			{
				this.IsBusy = false;
			}
        }

        public async void OnDataTypeSelected(object sender, EventArgs e)
        {
            if (!this.IsVisible)
            {
                return;
            }

            if (this.IsBusy)
            {
                return;
            }
            if (!this.isLoaded)
            {
                return;
            }

            await LoadRecords();


            UpdateDataGrid();
        }

        public async void OnSourceTypeSelected(object sender, EventArgs e)
        {
            if (!this.IsVisible)
            {
                return;
            }

            if (this.IsBusy)
            {
                return;
            }
            if (!this.isLoaded)
            {
                return;
            }
            
            stackFilter01.IsVisible = (this.sourceTypePicker.SelectedIndex != (int) SourceTypeSelected.selectTotals);

            await LoadRecords();


            UpdateDataGrid();
        }

        private void SetPeriodByType(PeriodTypeSelected periodTypeSelected )
		{
			endDatePicker.Date = DateTime.Now;
			startDatePicker.Date = endDatePicker.Date.AddDays(-1);

			switch (periodTypeSelected)
			{
				case PeriodTypeSelected.selectDay:
                    startDatePicker.Date = endDatePicker.Date.AddDays(-1);
					break;
				case PeriodTypeSelected.selectWeek:
                    startDatePicker.Date = endDatePicker.Date.AddDays(-7);
					break;
				case PeriodTypeSelected.selectWeekTwo:
                    startDatePicker.Date = endDatePicker.Date.AddDays(-14);
					break;
				case PeriodTypeSelected.selectMonth:
					startDatePicker.Date = endDatePicker.Date.AddDays(-30);
					break;
				case PeriodTypeSelected.selectMonthBegin:
					startDatePicker.Date = new DateTime(endDatePicker.Date.Year, endDatePicker.Date.Month, 1);
					break;
			}
		}

        protected async void OnRefresh()
        {
            if (!this.IsVisible)
            {
                return;
            }

            if (this.IsBusy)
            {
                return;
            }
            if (!this.isLoaded)
            {
                return;
            }

            await LoadRecords();


            UpdateDataGrid();
        }

		public async void OnPeriodReleativeSelected(object sender, EventArgs e)
		{
            if (!this.IsVisible)
            {
                return;
            }

            if (this.IsBusy)
            {
                return;
            }
            if (!this.isLoaded)
            {
                return;
            }

            SetPeriodByType((PeriodTypeSelected)periodRelativePicker.SelectedIndex);

			await LoadRecords();

			UpdateDataGrid();
		}

	}
}