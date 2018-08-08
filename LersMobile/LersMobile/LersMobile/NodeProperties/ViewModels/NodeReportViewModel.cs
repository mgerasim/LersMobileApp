using Android.Content;
using Lers.Administration;
using Lers.Core;
using Lers.Data;
using Lers.Reports;
using LersMobile.NodeProperties.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.NodeProperties.ViewModels
{
	public class NodeReportViewModel : INotifyPropertyChanged
    {
        private static DeviceDataType[] DataTypes = new DeviceDataType[]
        {
            DeviceDataType.Day, DeviceDataType.Hour, DeviceDataType.Month
        };

        public NodeReportViewModel(Node node, NodeReport nodeReport)
		{
            NodeReport = nodeReport;
            ReportCommand = new ReportCommand(this);
            _dateBgn = DateTime.Now.AddDays(-7);
            _dateEnd = DateTime.Now;
            Node = node;
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

        private string GetExtensionByFormat(ReportExportFormat format)
        {
            string extension = string.Empty;
            switch (format)
            {
                case ReportExportFormat.Csv:
                    extension = "csv";
                    break;
                case ReportExportFormat.Pdf:
                    extension = "pdf";
                    break;
                case ReportExportFormat.Rtf:
                    extension = "rtf";
                    break;
                case ReportExportFormat.Xls:
                    extension = "xls";
                    break;
                case ReportExportFormat.Xlsx:
                    extension = "xlsx";
                    break;
            }

            if (string.IsNullOrEmpty(extension))
            {
                throw new Exception(Droid.Resources.Messages.Text_Error_Could_not_determine_file_format);
            }

            return extension;
        }

        public async Task GenerateReport()
        {
            var reportExportOptions = new ReportExportOptions();
            reportExportOptions.Format = ReportExportFormat.Pdf;
            var reportManager = new ReportManager(App.Core.Server);

            var response = await reportManager.GenerateParametersSheetExportedAsync(
                reportExportOptions, 
                Node.Id,
                ReportEntity.Node, 
                NodeReport.Report.Id, 
                DataTypes[SelectedDataType],
                dateBgn, dateEnd);

            string extension = GetExtensionByFormat(reportExportOptions.Format);

            string fileName = response.FileName;
            fileName = Lers.Utils.FileUtils.SanitizeFileName(fileName);

            string directoryName = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + global::Android.OS.Environment.DirectoryDownloads;

            string fullName = Lers.Utils.FileUtils.CreateFullFileName(directoryName, fileName, extension);
                            
            File.WriteAllBytes(fullName, response.Content);
            
            Device.OpenUri(new Uri(fullName));

            return;
        }

    }
}