using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lers.Core;
using Lers.Reports;
using LersMobile.Views;

namespace LersMobile.Core.ReportLoader
{
    public class ReportLoaderNodes : IReportLoader
    {
        private List<NodeView> Nodes;

        private List<ReportViewCollectionGrouping> Reports { get; set; }

        public ReportLoaderNodes(List<NodeView> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException();
            }

            Nodes = nodes;

            Reports = new List<ReportViewCollectionGrouping>();
        }

        public async Task Reload(bool isForce = false)
        {
            Reports.Clear();

            List<ReportView> reportsAll = new List<ReportView>();

            foreach (var Node in Nodes)
            {
                var requiredFlags = NodeInfoFlags.Reports;

                if (!Node.Node.AvailableInfo.HasFlag(requiredFlags) || isForce == true)
                {
                    await Node.Node.RefreshAsync(requiredFlags);
                }

                foreach (var report in Node.Node.Reports)
                {
                    ReportView reportView = new ReportView(report.Report);
                    reportsAll.Add(reportView);
                }
                
            }

            foreach (ReportGroupType type in (ReportType[])Enum.GetValues(typeof(ReportGroupType)))
            {
                var list = reportsAll.Where(x => x.GroupType == type);

                if (list.Count() > 0)
                {
                    ReportViewCollectionGrouping item = new ReportViewCollectionGrouping(list.First().GroupType,
                        list.First().GroupTypeDescription);

                    foreach (var element in list)
                    {
                        if (item.Where(x => x.Id == element.Id).Count() == 0 )
                        {
                            item.Add(element);
                        }
                    }

                    Reports.Add(item);
                }                
            }

        }
        
        public List<ReportViewCollectionGrouping> GetReports()
        {
            return Reports;
        }

        public int[] GetEntitiesIds()
        {
            return Nodes.Select(x => x.Node.Id).ToArray();
        }

        public ReportEntity GetReportEntity()
        {
            return ReportEntity.Node;
        }
    }
}
