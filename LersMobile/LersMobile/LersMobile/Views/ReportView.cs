using Lers.Core;
using Lers.Reports;
using LersMobile.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Views
{
    public enum ReportGroupType
    {
        ParametersSheets = 0,
        Acts = 1,
        Passports = 2,
        SystemState = 3,
        NodeJob = 4,
        Calibration = 5,
        Others = 6
    }
    /// <summary>
    /// Сущность Отчёт для вывода на экран
    /// </summary>
    public class ReportView
    {
        public ReportView(int id, ReportType type, string title)
        {
            this.id = id;
            this.type = type;
            this.title = title;
            this.isAct = false;
        }

        public ReportView(Report report)
        {
            title = report.Title;
            id = report.Id;
            type = report.Type;
            isAct = report.IsAct;
        }

        #region Закрытые свойства

        private string title;

        private int id;

        private ReportType type;

        bool isAct;

        #endregion

        #region Свойства отчёта

        public string Title
        {
            get
            {
                return title;
            }
        }

        public ReportType Type
        {
            get
            {
                return type;
            }
        }


        public int Id
        {
            get
            {
                return id;
            }
        }

        public ReportGroupType GroupType => ReportUtils.GetReportGroupType(isAct, Type);

        public string GroupTypeDescription => ReportUtils.GetReportGroupDescription(GroupType);

        #endregion
    }
}
