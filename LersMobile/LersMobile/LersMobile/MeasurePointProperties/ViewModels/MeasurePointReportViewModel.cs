using Lers.Core;
using Lers.Reports;
using LersMobile.Core;
using LersMobile.MeasurePointProperties.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace LersMobile.MeasurePointProperties.ViewModels
{
    public class MeasurePointReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private MeasurePoint MeasurePoint;

        private MeasurePointReport MeasurePointReport;

        public ReportCommand ReportCommand { get; set; }

        public MeasurePointReportViewModel(MeasurePoint measurePoint, MeasurePointReport measurePointReport)
        {
            MeasurePoint = measurePoint;

            MeasurePointReport = measurePointReport;

            _dateBgn = DateTime.Now.AddDays(-7);
            _dateEnd = DateTime.Now;

            ReportCommand = new ReportCommand(this);
        }

        public string Title
        {
            get
            {
                return MeasurePointReport.Report.Title;
            }
        }

        private bool _isBusy;

        public bool isBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged("isBusy");
            }
        }

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
        private int _selectedFileFormat;

        public int SelectedFileFormat
        {
            get
            {
                return _selectedFileFormat;
            }
            set
            {
                _selectedFileFormat = value;
                OnPropertyChanged("SelectedFileFormat");
            }
        }

        public async Task GenerateReport()
        {
            var reportExportOptions = new ReportExportOptions();
            reportExportOptions.Format = (ReportExportFormat)SelectedFileFormat;
            var reportManager = new ReportManager(App.Core.Server);

            var response = await reportManager.GenerateParametersSheetExportedAsync(
                reportExportOptions,
                MeasurePoint.Id,
                ReportEntity.MeasurePoint,
                MeasurePointReport.Report.Id,
                ReportUtils.DataTypes[SelectedDataType],
                dateBgn, dateEnd);

            ReportUtils.SaveResponse(response, reportExportOptions.Format);
        }

    }
}
