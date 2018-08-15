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
        Others = 3
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

        public ReportGroupType GroupType
        {
            get
            {
                return ReportUtils.GetReportGroupType(isAct, Type);
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
