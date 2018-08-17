using Lers.Core;
using System.Collections.Generic;

namespace LersMobile.Views
{
	/// <summary>
	/// Коллекция отчётов 
	/// </summary>
	public class ReportViewCollection : List<ReportView>
	{
		/// <summary>
		/// Строит коллекцию на основе коллекции отчетов объекта учета
		/// </summary>
		/// <param name="nodeReports"></param>
		public void Reload(NodeReportCollection nodeReports)
		{
			Clear();
			foreach (var report in nodeReports)
			{
				ReportView item = new ReportView(report.Report);
				Add(item);
			}
		}

		/// <summary>
		/// Строит коллекцию на основе коллекции отчетов точки учета
		/// </summary>
		/// <param name="measurePointReports"></param>
		public void Reload(MeasurePointReportCollection measurePointReports)
		{
			Clear();
			foreach (var report in measurePointReports)
			{
				ReportView item = new ReportView(report.Report);
				Add(item);
			}
		}
	}
}
