using Lers.Reports;
using LersMobile.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LersMobile.Core.ReportLoader
{
	/// <summary>
	/// Интерфейс загрузки данных по отчетам для страницы ReportsPage
	/// </summary>
    public interface IReportLoader
    {
		/// <summary>
		/// Загрузка данных
		/// </summary>
		/// <param name="isForce"></param>
		/// <returns></returns>
        Task Reload(bool isForce = false);
		/// <summary>
		/// Возвращает на источник данных отчётов
		/// </summary>
		/// <returns></returns>
        List<ReportsView> GetReports();
		/// <summary>
		/// Список идентификаторов сущностей
		/// </summary>
		/// <returns></returns>
        int[] GetEntitiesIds();
		/// <summary>
		/// Тип сущности
		/// </summary>
		/// <returns></returns>
        ReportEntity GetReportEntity();
    }
}
