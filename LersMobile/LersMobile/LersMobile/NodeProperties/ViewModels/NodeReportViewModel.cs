using Lers.Core;
using LersMobile.NodeProperties.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.NodeProperties.ViewModels
{
	public class NodeReportViewModel 
	{
		public NodeReportViewModel(NodeReport nodeReport)
		{
            NodeReport = nodeReport;
            ReportCommand = new ReportCommand(this);
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
        
        public void GenerateReport()
        {
            return;
        }

    }
}