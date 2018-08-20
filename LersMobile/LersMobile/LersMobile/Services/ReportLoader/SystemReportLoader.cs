using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lers.Reports;
using LersMobile.Views;

namespace LersMobile.Core.ReportLoader
{
	/// <summary>
	/// Реализация загрузки системных отчетов
	/// </summary>
    public class SystemReportLoader : IReportLoader
    {
        public SystemReportLoader()
        {
            Reports = new List<ReportsView>();
        }

        private static ReportType[] ReportTypeFilter = new ReportType[] { ReportType.SystemState, ReportType.NodeJob, ReportType.Calibration };

        protected List<ReportsView> Reports { get; set; }

        public int[] GetEntitiesIds()
        {
            return new int[] { -1 };
        }

        public ReportEntity GetReportEntity()
        {
            return ReportEntity.System;
        }

        public List<ReportsView> GetReports()
        {
            return Reports;
        }

        public async Task Reload(bool isForce = false)
        {
            try
            {
                Reports.Clear();

                var reportManager = new ReportManager(App.Core.Server);

                var reportList = await reportManager.GetReportListAsync();
                
                var reportsAll = new List<ReportView>();

                foreach (var report in reportList)
                {
                    var reportView = new ReportView(report);
                    reportsAll.Add(reportView);
                }                

                foreach (var type in ReportTypeFilter)
                {
                    var list = reportsAll.Where(x => x.Type == type);

                    if (list.Count() > 0)
                    {
						var item = new ReportsView(list.First().GroupType,
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
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error_Load, ex.Message, "Ok");
            }
        }
    }
}
