using Lers.Core;
using Lers.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Entities
{
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

        public ReportType Type => this.Report.Type;

        public int Id => this.Report.Id;

        #endregion
    }
}
