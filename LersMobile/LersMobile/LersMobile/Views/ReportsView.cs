using System.Collections.ObjectModel;
using LersMobile.Services.Report;

namespace LersMobile.Views
{    
    /// <summary>
    /// Коллекция отчётов сгрупированные по типу
    /// </summary>
    public class ReportsView : ObservableCollection<ReportView>
    {
		/// <summary>
		/// Наименование группы отчетов
		/// </summary>
        public string GroupTypeDescription { get; protected set; } = string.Empty;

		/// <summary>
		/// Идентификатор по которому происходит группировка в группу
		/// </summary>
        public ReportGroupType GroupType { get; protected set; } = 0;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="reportEntity"></param>
        public ReportsView(ReportView reportEntity)
        {
            GroupType = reportEntity.GroupType;
            GroupTypeDescription = reportEntity.GroupTypeDescription;
        }
        
		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
        public ReportsView()
        {
            GroupTypeDescription = Droid.Resources.Messages.Text_Default;
        }

		/// <summary>
		/// Конструктор с описанием группы
		/// </summary>
		/// <param name="type"></param>
		/// <param name="desc"></param>
        public ReportsView(ReportGroupType type, string desc)
        {
            GroupType = type;
            GroupTypeDescription = desc;
        }

    }
}
