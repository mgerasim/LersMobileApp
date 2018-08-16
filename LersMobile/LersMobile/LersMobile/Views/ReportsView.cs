using Lers.Core;
using Lers.Reports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LersMobile.Views
{
    /// <summary>
    /// Коллекция отчётов для вывода на экран
    /// </summary>
    public class ReportViewCollection : List<ReportView>
    {
        public ReportViewCollection()
        {

        }

        public void Reload(NodeReportCollection nodeReports)
        {
            Clear();
            foreach (var report in nodeReports)
            {
                ReportView item = new ReportView(report.Report);
                Add(item);
            }
        }

        public void Reload(MeasurePointReportCollection measurePointReports)
        {
            Clear();
            foreach(var report in measurePointReports)
            {
                ReportView item = new ReportView(report.Report);
                Add(item);
            }
        }
    }

    /// <summary>
    /// Коллекция отчётов сгрупированные по типу
    /// </summary>
    public class ReportsView : ObservableCollection<ReportView>
    {
        public string GroupTypeDescription { get; protected set; } = string.Empty;

        public ReportGroupType GroupType { get; protected set; } = 0;

        public ReportsView(ReportView reportEntity)
        {
            GroupType = reportEntity.GroupType;
            GroupTypeDescription = reportEntity.GroupTypeDescription;
        }
        
        public ReportsView()
        {
            GroupTypeDescription = Droid.Resources.Messages.Text_Default;
        }

        public ReportsView(ReportGroupType type, string desc)
        {
            GroupType = type;
            GroupTypeDescription = desc;
        }

    }
}
