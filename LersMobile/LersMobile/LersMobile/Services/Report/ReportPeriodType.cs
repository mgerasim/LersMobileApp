
namespace LersMobile.Services.Report
{
	/// <summary>
	/// Предопределнные периоды в фильтрации архивных данных при генерации отчетов
	/// </summary>
    public enum ReportPeriodType
	{
		/// <summary>
		/// За сутки
		/// </summary>
		Day = 0,
		/// <summary>
		/// За неделю
		/// </summary>
		Week = 1,
		/// <summary>
		/// За две недели
		/// </summary>
		WeekTwo = 2,
		/// <summary>
		/// За месяц
		/// </summary>
		Month = 3,
		/// <summary>
		/// С начала месяца
		/// </summary>
		MonthBegin = 4
	}


}
