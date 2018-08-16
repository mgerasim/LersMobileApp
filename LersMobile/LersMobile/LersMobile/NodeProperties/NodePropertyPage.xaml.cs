using LersMobile.Core.ReportLoader;
using LersMobile.Pages.ReportsPage;
using LersMobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.NodeProperties
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NodePropertyPage : TabbedPage
    {
        public NodePropertyPage(NodeView nodeView)
        {
            InitializeComponent();

            this.Children.Add(new NodeCommonPropertiesPage(nodeView));
            this.Children.Add(new NodeMeasurePointsPage(nodeView));
            IReportLoader reportLoader = new ReportLoaderNode(nodeView);
            this.Children.Add(new ReportsPage(reportLoader));
            this.Title = Droid.Resources.Messages.NodePropertyPage_Title;
		}
    }
}