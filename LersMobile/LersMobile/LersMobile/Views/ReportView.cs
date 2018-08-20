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

		#region Закрытые свойства

		private string _title;

		private int _id;

		private ReportType _type;

		bool _isAct;

		#endregion

		#region Свойства отчёта

		public string Title => _title;

		public ReportType Type => _type;

		public int Id => _id;
		
		public ReportGroupType GroupType => ReportService.GetReportGroupType(_isAct, Type);

		public string GroupTypeDescription => ReportService.GetReportGroupDescription(GroupType);
		
		#endregion

		public ReportView(int id, ReportType type, string title)
        {
            this._id = id;
            this._type = type;
            this._title = title;
            this._isAct = false;
        }

        public ReportView(Report report)
        {
            _title = report.Title;
            _id = report.Id;
            _type = report.Type;
            _isAct = report.IsAct;
        }
    }
}
