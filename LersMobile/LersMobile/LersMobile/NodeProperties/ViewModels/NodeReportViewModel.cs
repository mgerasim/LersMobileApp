using Lers.Core;
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

		}

        private NodeReport NodeReport { get; set; }

        public string Title
        {
            get
            {
                return NodeReport.Report.Title;
            }
        }
	}
}