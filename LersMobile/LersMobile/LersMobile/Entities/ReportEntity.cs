using Lers.Core;
using Lers.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Entities
{
    public enum ReportGroupType
    {
        ParametersSheets = 0,
        Acts = 1,
        Passports = 2,
        Others = 3
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
                    return ReportGroupType.Acts;
                }
                switch (this.Report.Type)
                {
                    case Lers.Reports.ReportType.ParametersSheet:
                        return ReportGroupType.ParametersSheets;
                    case ReportType.NodePassport:
                        return ReportGroupType.Passports;
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
                    case ReportGroupType.Acts:
                        return Droid.Resources.Messages.Text_Acts;
                    case ReportGroupType.ParametersSheets:
                        return Droid.Resources.Messages.Text_ParametersSheet;
                    case ReportGroupType.Passports:
                        return Droid.Resources.Messages.Text_Passports;
                }

                return Droid.Resources.Messages.Text_Others;
            }
        }

        #endregion
    }
}
