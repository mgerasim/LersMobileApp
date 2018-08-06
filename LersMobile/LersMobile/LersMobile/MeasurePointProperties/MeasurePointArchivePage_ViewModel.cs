using Lers.Data;
using Lers.Utils;
using LersMobile.MeasurePointProperties.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace LersMobile.MeasurePointProperties
{
    enum PeriodType
    {
        Day = 0,
        Week = 1,
        WeekTwo = 2,
        Month = 3,
        MonthBegin = 4
    }

    enum SourceType
    {
        Consumption = 0,    // Потребление
        Totals = 1          // Интеграторы
    }

    public class MeasurePointArchivePage_ViewModel : INotifyPropertyChanged
    {
        private static String[] DaviceDataTypeDateStringFormat = new String[]
        {
            "{0:dd.MM.yyyy}", "{0:dd.MM.yyyy HH:mm}", "{0:MM.yyyy}"
        };

        private static DeviceDataType[] DataTypes = new DeviceDataType[]
        {
            DeviceDataType.Day, DeviceDataType.Hour, DeviceDataType.Month
        };

        private String CurrentDaviceDataTypeDateStringFormat => DaviceDataTypeDateStringFormat[_selectedDataType];

        public bool IsSourceTypeEqConsumption
        {
            get
            {
                return (SelectedSourceType == (int)SourceType.Consumption);
            }
        }

        public MeasurePointArchivePage_ViewModel(StackLayout stackLayoutData, Xamarin.Forms.DataGrid.DataGrid dataGrid, Lers.Core.MeasurePoint measurePoint)
        {
            _dateBgn = DateTime.Now;
            _dateEnd = DateTime.Now;
            _selectedDataType = 0;
            this.stackLayoutData = stackLayoutData;
            this.dataGrid = dataGrid;
            MeasurePoint = measurePoint;
            LoadCommand = new LoadCommand(this);
            ShowCommand = new ShowCommand(this);
            IsShowed = true;
        }

        public bool IsBusy { get; set; }
        
        private Lers.Core.MeasurePoint MeasurePoint;

        private StackLayout stackLayoutData;

        private Xamarin.Forms.DataGrid.DataGrid dataGrid;

        public LoadCommand LoadCommand { get; set; }

        public ShowCommand ShowCommand { get; set; }

        public bool IsShowed { get; set; }

        private int _selectedPeriodType;

        public int SelectedPeriodType
        {
            get
            {
                return _selectedPeriodType;
            }
            set
            {
                _selectedPeriodType = value;
                _updatePeriod();
                OnPropertyChanged("SelectedPeriodType");
            }
        }

        private int _selectedDataType;

        public int SelectedDataType
        {
            get
            {
                return _selectedDataType;
            }
            set
            {
                _selectedDataType = value;
                OnPropertyChanged("SelectedDataType");
            }
        }
        
        private int _selectedSourceType;

        public int SelectedSourceType
        {
            get
            {
                return _selectedSourceType;
            }
            set
            {
                _selectedSourceType = value;
                OnPropertyChanged("SelectedSourceType");
                OnPropertyChanged("IsSourceTypeEqConsumption");
            }
        }

        public ObservableCollection<DataRecord> Data { get; private set; } = new ObservableCollection<DataRecord>();

        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _dateBgn;

        public DateTime dateBgn
        {
            get
            {
                return _dateBgn;
            }
            set
            {
                _dateBgn = value;
                OnPropertyChanged("dateBgn");
            }
        }

        private DateTime _dateEnd;

        public DateTime dateEnd
        {
            get
            {
                return _dateEnd;
            }
            set
            {
                _dateEnd = value;
                OnPropertyChanged("dateEnd");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ShowFilter()
        {
            IsShowed = !IsShowed;
            OnPropertyChanged("IsShowed");
        }

        public async void LoadData()
        {            
            try
            {
                IsBusy = true;

                await App.Core.EnsureConnected();

                this.Data.Clear();

                switch (SelectedSourceType)
                {
                    case (int)SourceType.Consumption:
                        this.Data.AddRange((await this.MeasurePoint.Data.GetConsumptionAsync(dateBgn, dateEnd, DataTypes[SelectedDataType])).OrderByDescending(x => x.DateTime));
                        break;
                    case (int)SourceType.Totals:
                        this.Data.AddRange((await this.MeasurePoint.Data.GetTotalsAsync(dateBgn, dateEnd)).OrderByDescending(x => x.DateTime));
                        break;

                }

                _dataGridRefreshHead();
                _dataGridRefreshBody();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void _dataGridRefreshBody()
        {
            stackLayoutData.Children.Clear();

            if (SelectedSourceType == (int)SourceType.Totals)
            {
                dataGrid.Columns[0].StringFormat = "{0:dd.MM.yyyy HH:mm}";
            }
            else
            {
                dataGrid.Columns[0].StringFormat = CurrentDaviceDataTypeDateStringFormat;
            }

            stackLayoutData.Children.Add(dataGrid);
        }

        private void _dataGridRefreshHead()
        {
            dataGrid.Columns.Clear();

            Xamarin.Forms.DataGrid.DataGridColumn columnDateTime = new Xamarin.Forms.DataGrid.DataGridColumn();

            columnDateTime.Title = Droid.Resources.Messages.Text_Date;
            columnDateTime.PropertyName = "DateTime";

            dataGrid.Columns.Add(columnDateTime);

            foreach (var param in MeasurePoint.DataParameters)
            {
                if (SelectedSourceType == (int)SourceType.Totals && !DataParameterDescriptor.Get(param).IsAdditive)
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

        private void _updatePeriod()
        {
            dateEnd = DateTime.Now;

            switch (SelectedPeriodType)
            {
                case (int)PeriodType.Day:
                    dateBgn = dateEnd.AddDays(-1);
                    break;
                case (int)PeriodType.Week:
                    dateBgn = dateEnd.AddDays(-7);
                    break;
                case (int)PeriodType.WeekTwo:
                    dateBgn = dateEnd.AddDays(-14);
                    break;
                case (int)PeriodType.Month:
                    dateBgn = dateEnd.AddDays(-30);
                    break;
                case (int)PeriodType.MonthBegin:
                    dateBgn = new DateTime(dateEnd.Date.Year, dateEnd.Month, 1);
                    break;
            }
        }
    }
}
