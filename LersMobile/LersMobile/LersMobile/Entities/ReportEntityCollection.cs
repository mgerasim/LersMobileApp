using Lers.Core;
using Lers.Reports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LersMobile.Entities
{
    /// <summary>
    /// Коллекция отчётов для вывода на экран
    /// </summary>
    public class ReportEntityCollection : List<ReportEntity>
    {
        public ReportEntityCollection()
        {

        }

        public void Reload(NodeReportCollection nodeReports)
        {
            Clear();
            foreach (var report in nodeReports)
            {
                ReportEntity item = new ReportEntity(report);
                Add(item);
            }
        }

        public void Reload(MeasurePointReportCollection measurePointReports)
        {
            Clear();
            foreach(var report in measurePointReports)
            {
                ReportEntity item = new ReportEntity(report);
                Add(item);
            }
        }
    }

    /// <summary>
    /// Коллекция отчётов сгрупированные по типу
    /// </summary>
    public class ReportEntityCollectionGrouping : ObservableCollection<ReportEntity>
    {
        public string GroupTypeDescription { get; protected set; }

        public ReportGroupType GroupType { get; protected set; }

        public ReportEntityCollectionGrouping(ReportEntity reportEntity)
        {
            GroupType = reportEntity.GroupType;
            GroupTypeDescription = reportEntity.GroupTypeDescription;
        }
        
    }
}
