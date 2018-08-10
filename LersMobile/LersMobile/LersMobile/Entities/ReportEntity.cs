using Lers.Core;
using Lers.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Entities
{
    public enum ReportGroupType
    {
        Act = 0,
        ParametersSheet = 1,
        Others = 2
    }
    /// <summary>
    /// Сущность Отчёт для вывода на экран
    /// </summary>
    public class ReportEntity
    {
        public ReportEntity(NodeReport report)
        {
            Report = report.Report;
        }

        public ReportEntity(MeasurePointReport report)
        {
            Report = report.Report;
        }

        private Report Report { get; set; }

        #region Свойства отчёта

        public string Title
        {
            get
            {
                return Report.Title;
            }
        }

        public Lers.Reports.ReportType Type => this.Report.Type;

        public int Id => this.Report.Id;

        public ReportGroupType GroupType
        {
            get
            {
                if (this.Report.IsAct)
                {
                    return ReportGroupType.Act;
                }
                switch (this.Report.Type)
                {
                    case Lers.Reports.ReportType.ParametersSheet:
                        return ReportGroupType.ParametersSheet;
                }

                return ReportGroupType.Others;
            }
        }

        public string GroupTypeDescription
        {
            get
            {
                switch (GroupType)
                {
                    case ReportGroupType.Act:
                        return Droid.Resources.Messages.Text_Act;
                    case ReportGroupType.ParametersSheet:
                        return Droid.Resources.Messages.Text_ParametersSheet;
                }
                return Droid.Resources.Messages.Text_Others;
            }
        }

        #endregion
    }
}
