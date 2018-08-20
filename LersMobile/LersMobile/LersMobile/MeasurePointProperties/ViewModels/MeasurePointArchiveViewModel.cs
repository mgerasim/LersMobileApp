using Lers.Data;
using Lers.Utils;
using LersMobile.MeasurePointProperties.ViewModels.Commands;
using LersMobile.Services.Report;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LersMobile.MeasurePointProperties.ViewModels
{
	/// <summary>
	/// Класс модели представления
	/// </summary>
    public class MeasurePointArchiveViewModel : INotifyPropertyChanged
    {
		/// <summary>
		/// Формат отображения даты в зависимости от типа данных
		/// </summary>
        private static String[] DaviceDataTypeDateStringFormat = new String[]
        {
            "{0:dd.MM.yyyy}", "{0:dd.MM.yyyy HH:mm}", "{0:MM.yyyy}"
        };
		/// <summary>
		/// Типы данных: суточные, часовые, месячные
		/// </summary>
        private static DeviceDataType[] DataTypes = new DeviceDataType[]
        {			
            DeviceDataType.Day,	DeviceDataType.Hour,DeviceDataType.Month
        };

		/// <summary>
		/// Выбранный тип данных
		/// </summary>
        private String CurrentDaviceDataTypeDateStringFormat => DaviceDataTypeDateStringFormat[_selectedDataType];

		/// <summary>
		/// Признак того, что источником данных является Потребление - используется для скрытия некоторых полей
		/// </summary>
        public bool IsSourceTypeEqConsumption
        {
            get => (SelectedSourceType == (int)ReportSourceType.Consumption);
        }

		/// <summary>
		/// Признак того, что идет обновление данных
		/// </summary>
        public bool IsBusy { get; set; }
        
		/// <summary>
		/// Точка учёта для которой, генерируется отчёт
		/// </summary>
        private Lers.Core.MeasurePoint _measurePoint;

		/// <summary>
		/// Контейнер таблицы данных
		/// </summary>
        private StackLayout _stackLayoutData;

		/// <summary>
		/// Элемент управления таблицы данных
		/// </summary>
        private Xamarin.Forms.DataGrid.DataGrid _dataGrid;

		/// <summary>
		/// Команда загрузки данных
		/// </summary>
        public LoadCommand LoadCommand { get; set; }

		/// <summary>
		/// Команда отображения панели фильтрации
		/// </summary>
        public ShowCommand ShowCommand { get; set; }

		/// <summary>
		/// Признак того, что панель фильтрации отображается
		/// </summary>
        public bool IsShowed { get; set; }

		/// <summary>
		/// Выбранный преопределённый период 
		/// </summary>
        private int _selectedPeriodType;

        public int SelectedPeriodType
        {
            get => _selectedPeriodType;
            set
            {
                _selectedPeriodType = value;
                UpdatePeriod();
                OnPropertyChanged(nameof(SelectedPeriodType));
            }
        }

		/// <summary>
		/// Выбранный тип данных
		/// </summary>
        private int _selectedDataType;

        public int SelectedDataType
        {
            get => _selectedDataType;
            set
            {
                _selectedDataType = value;
                OnPropertyChanged(nameof(SelectedDataType));
            }
        }
        
		/// <summary>
		/// Выбранный источник данных
		/// </summary>
        private int _selectedSourceType;

        public int SelectedSourceType
        {
            get => _selectedSourceType;
            set
            {
                _selectedSourceType = value;
                OnPropertyChanged(nameof(SelectedSourceType));
                OnPropertyChanged(nameof(IsSourceTypeEqConsumption));
            }
        }

		/// <summary>
		/// Дата начало периода выборки архивных данных
		/// </summary>
        private DateTime _dateStart;

        public DateTime DateStart
        {
            get => _dateStart;
            set
            {
                _dateStart = value;
                OnPropertyChanged(nameof(DateStart));
            }
        }

		/// <summary>
		/// Дата окончания периода выборки архивных данных
		/// </summary>
        private DateTime _dateEnd;

        public DateTime DateEnd
        {
            get =>_dateEnd;
            set
            {
                _dateEnd = value;
                OnPropertyChanged(nameof(DateEnd));
            }
        }

		/// <summary>
		/// Источник данных
		/// </summary>
		public ObservableCollection<DataRecord> Data { get; private set; } = new ObservableCollection<DataRecord>();

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="stackLayoutData"></param>
		/// <param name="dataGrid"></param>
		/// <param name="measurePoint"></param>
		public MeasurePointArchiveViewModel(StackLayout stackLayoutData, Xamarin.Forms.DataGrid.DataGrid dataGrid, Lers.Core.MeasurePoint measurePoint)
        {
            _dateStart = DateTime.Now;
            _dateEnd = DateTime.Now;
            _selectedDataType = 0;
            this._stackLayoutData = stackLayoutData;
            this._dataGrid = dataGrid;
            _measurePoint = measurePoint;
            LoadCommand = new LoadCommand(this);
            ShowCommand = new ShowCommand(this);
            IsShowed = true;
        }
		        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

		/// <summary>
		/// Обработчик отображения панели фильтрации
		/// </summary>
        public void ShowFilter()
        {
            IsShowed = !IsShowed;
            OnPropertyChanged(nameof(IsShowed));
        }

		/// <summary>
		/// Обработчик загрузки данных
		/// </summary>
		/// <returns></returns>
        public async Task LoadData()
        {            
            try
            {
                IsBusy = true;

                await App.Core.EnsureConnected();

                this.Data.Clear();

                switch (SelectedSourceType)
                {
                    case (int)ReportSourceType.Consumption:
                        this.Data.AddRange((await this._measurePoint.Data.GetConsumptionAsync(DateStart, DateEnd, DataTypes[SelectedDataType])).OrderByDescending(x => x.DateTime));
                        break;
                    case (int)ReportSourceType.Totals:
                        this.Data.AddRange((await this._measurePoint.Data.GetTotalsAsync(DateStart, DateEnd)).OrderByDescending(x => x.DateTime));
                        break;
                }

                DataGridRefreshHead();
                DataGridRefreshBody();
            }
            finally
            {
                IsBusy = false;
            }
        }

		/// <summary>
		/// Заполняет таблицу строками с данными
		/// </summary>
        private void DataGridRefreshBody()
        {
            _stackLayoutData.Children.Clear();

            if (SelectedSourceType == (int)ReportSourceType.Totals)
            {
                _dataGrid.Columns[0].StringFormat = "{0:dd.MM.yyyy HH:mm}";
            }
            else
            {
                _dataGrid.Columns[0].StringFormat = CurrentDaviceDataTypeDateStringFormat;
            }

            _stackLayoutData.Children.Add(_dataGrid);
        }
		/// <summary>
		/// Формирует колонки для таблицы с данными
		/// </summary>
        private void DataGridRefreshHead()
        {
            _dataGrid.Columns.Clear();

            Xamarin.Forms.DataGrid.DataGridColumn columnDateTime = new Xamarin.Forms.DataGrid.DataGridColumn();

            columnDateTime.Title = Droid.Resources.Messages.Text_Date;
            columnDateTime.PropertyName = "DateTime";

            _dataGrid.Columns.Add(columnDateTime);

            foreach (var param in _measurePoint.DataParameters)
            {
                if (SelectedSourceType == (int)ReportSourceType.Totals && !DataParameterDescriptor.Get(param).IsAdditive)
                {
                    continue;
                }

                Xamarin.Forms.DataGrid.DataGridColumn columnItem = new Xamarin.Forms.DataGrid.DataGridColumn();

                var desc = DataParameterDescriptor.Get(param);

                columnItem.Title = desc.ShortTitle;
                columnItem.PropertyName = desc.Name;
                columnItem.StringFormat = "{0:0.00}";

                _dataGrid.Columns.Add(columnItem);
            }
        }

		/// <summary>
		/// Обновляет даты абсолютного периода в зависимости изменения преодпределенного периода
		/// </summary>
        private void UpdatePeriod()
        {
            DateEnd = DateTime.Now;

            switch (SelectedPeriodType)
            {
                case (int)ReportPeriodType.Day:
                    DateStart = DateEnd.AddDays(-1);
                    break;
                case (int)ReportPeriodType.Week:
                    DateStart = DateEnd.AddDays(-7);
                    break;
                case (int)ReportPeriodType.WeekTwo:
                    DateStart = DateEnd.AddDays(-14);
                    break;
                case (int)ReportPeriodType.Month:
                    DateStart = DateEnd.AddDays(-30);
                    break;
                case (int)ReportPeriodType.MonthBegin:
                    DateStart = new DateTime(DateEnd.Date.Year, DateEnd.Month, 1);
                    break;
            }
        }
    }
}
