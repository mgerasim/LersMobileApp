using Android.Content;
using Lers.Administration;
using Lers.Core;
using Lers.Data;
using Lers.Reports;
using LersMobile.Core;
using LersMobile.NodeProperties.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LersMobile.NodeProperties.ViewModels
{
	public class NodeReportViewModel : INotifyPropertyChanged
    {
        public NodeReportViewModel(Node node, NodeReport nodeReport)
		{
            NodeReport = nodeReport;
            ReportCommand = new ReportCommand(this);
            _dateBgn = DateTime.Now.AddDays(-7);
            _dateEnd = DateTime.Now;
            Node = node;
            _isBusy = false;
		}

        public ReportCommand ReportCommand { get; set; }

        private NodeReport NodeReport { get; set; }

        private Node Node;

        public string Title
        {
            get
            {
                return NodeReport.Report.Title;
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
            try
            {
                if (isBusy)
                {
                    return;
                }

                isBusy = true;

                var reportExportOptions = new ReportExportOptions();
                reportExportOptions.Format = (ReportExportFormat)SelectedFileFormat;
                var reportManager = new ReportManager(App.Core.Server);

                var response = await reportManager.GenerateParametersSheetExportedAsync(
                    reportExportOptions,
                    Node.Id,
                    ReportEntity.Node,
                    NodeReport.Report.Id,
                    ReportUtils.DataTypes[SelectedDataType],
                    dateBgn, dateEnd);

                isBusy = false;

                ReportUtils.SaveResponse(response, reportExportOptions.Format);
            }
            catch (Exception ex)
            {
                isBusy = false;
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error, ex.Message, "OK");
            }
            finally
            {
                isBusy = false;
            }
        }

    }
}