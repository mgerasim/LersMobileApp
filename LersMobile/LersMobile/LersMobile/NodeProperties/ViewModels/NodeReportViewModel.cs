using Lers.Administration;
using Lers.Core;
using LersMobile.NodeProperties.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.NodeProperties.ViewModels
{
	public class NodeReportViewModel : INotifyPropertyChanged
    {
		public NodeReportViewModel(NodeReport nodeReport)
		{
            NodeReport = nodeReport;
            ReportCommand = new ReportCommand(this);
            _dateBgn = DateTime.Now.AddDays(-7);
            _dateEnd = DateTime.Now;
		}

        public ReportCommand ReportCommand { get; set; }

        private NodeReport NodeReport { get; set; }

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

        public void GenerateReport()
        {



            return;
        }

    }
}