using Lers.Reports;
using LersMobile.Core;
using LersMobile.Services.Report;

namespace LersMobile.Views
{
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
