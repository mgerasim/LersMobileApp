using Lers.Core;
using LersMobile.NodeProperties.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.NodeProperties
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NodeReportPage : ContentPage
	{
        public NodeReport NodeReport;

        public NodeReportViewModel ViewModel;

		public NodeReportPage(NodeReport nodeReport)
		{
			InitializeComponent();

            NodeReport = nodeReport;

            ViewModel = new NodeReportViewModel(NodeReport);
            BindingContext = ViewModel;
		}
	}
}