using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lers.Reports;
using LersMobile.Views;

namespace LersMobile.Core.ReportLoader
{
    public class ReportLoaderMeasurePoint : IReportLoader
    {
        private ReportLoaderNodesMeasurePoints ReportLoaderNodesMeasurePoints;

        public ReportLoaderMeasurePoint(MeasurePointView measurePointView)
        {
            var measurePointViews = new List<MeasurePointView>();
            measurePointViews.Add(measurePointView);

            ReportLoaderNodesMeasurePoints = new ReportLoaderNodesMeasurePoints(measurePointViews);
        }

        public int[] GetEntitiesIds()
        {
            return ReportLoaderNodesMeasurePoints.GetEntitiesIds();
        }

        public ReportEntity GetReportEntity()
        {
            return ReportLoaderNodesMeasurePoints.GetReportEntity();
        }

        public List<ReportsView> GetReports()
        {
            return ReportLoaderNodesMeasurePoints.GetReports();
        }

        public async Task Reload(bool isForce = false)
        {
            await ReportLoaderNodesMeasurePoints.Reload(isForce);
        }
    }
}
