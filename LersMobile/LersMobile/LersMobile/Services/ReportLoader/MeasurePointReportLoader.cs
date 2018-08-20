using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lers.Reports;
using LersMobile.Views;

namespace LersMobile.Core.ReportLoader
{
	/// <summary>
	/// Реализация загрузки отчетов для точки учета
	/// </summary>
    public class MeasurePointReportLoader : IReportLoader
    {
        private NodesMeasurePointsReportLoader ReportLoaderNodesMeasurePoints;

        public MeasurePointReportLoader(MeasurePointView measurePointView)
        {
            var measurePointViews = new List<MeasurePointView>();
            measurePointViews.Add(measurePointView);

            ReportLoaderNodesMeasurePoints = new NodesMeasurePointsReportLoader(measurePointViews);
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
