using Lers.Core;
using Lers.Reports;
using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LersMobile.Core.ReportLoader
{
    public class ReportLoaderNodesMeasurePoints : IReportLoader
    {
        protected List<MeasurePointView> MeasurePointViews;

        protected List<ReportsView> Reports { get; set; }

        public ReportLoaderNodesMeasurePoints(List<MeasurePointView> measurePointViews) 
        {
            MeasurePointViews = measurePointViews ?? throw new ArgumentNullException();

            Reports = new List<ReportsView>();
        }

        public async Task Reload(bool isForce = false)
        {
            Reports.Clear();

            List<ReportView> reportsAll = new List<ReportView>();

            foreach (var measurePointView in MeasurePointViews)
            {
                var requiredFlags = MeasurePointInfoFlags.Reports;

                if (!measurePointView.MeasurePoint.AvailableInfo.HasFlag(requiredFlags) || isForce == true)
                {
                    await measurePointView.MeasurePoint.RefreshAsync(requiredFlags);
                }

                foreach (var report in measurePointView.MeasurePoint.Reports)
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
					ReportsView item = new ReportsView(list.First().GroupType,
                        list.First().GroupTypeDescription);

                    foreach (var element in list)
                    {
                        if (item.Where(x => x.Id == element.Id).Count() == 0)
                        {
                            item.Add(element);
                        }
                    }

                    Reports.Add(item);
                }
            }
        }

        public List<ReportsView> GetReports()
        {
            return Reports;
        }

        public int[] GetEntitiesIds()
        {
            return MeasurePointViews.Select(x => x.MeasurePoint.Id).ToArray();
        }

        public ReportEntity GetReportEntity()
        {
            return ReportEntity.MeasurePoint;
        }
    }
}
