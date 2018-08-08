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

        public Node Node;

        public NodeReportViewModel ViewModel;

		public NodeReportPage(Node node, NodeReport nodeReport)
		{
			InitializeComponent();

            Node = node;

            NodeReport = nodeReport;
            
            Title = Droid.Resources.Messages.Text_Report;

            ViewModel = new NodeReportViewModel(Node, NodeReport);
            BindingContext = ViewModel;
		}

	}
}