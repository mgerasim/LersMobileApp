using Lers.Data;
using Lers.Reports;
using LersMobile.Core.ReportLoader;
using LersMobile.Pages.ReportsPage;
using LersMobile.Services.StorageDirectory;
using System;
using System.IO;
using Xamarin.Forms;

namespace LersMobile.Services.Report
{
	/// <summary>
	/// Сервис для работы с отчетами
	/// </summary>
    public static class ReportService
    {
		/// <summary>
		/// Типы данных, используемых для установки фильтрации данных для генерации отчета
		/// </summary>
		public static DeviceDataType[] DataTypes = new DeviceDataType[]
		{
			DeviceDataType.Day, DeviceDataType.Hour, DeviceDataType.Month
		};
		/// <summary>
		/// Возвращает расширение файла по элементу перечисления ReportExportFormat типов файлов
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string GetExtensionByFormat(ReportExportFormat format)
		{
			string extension = string.Empty;

			switch (format)
			{
				case ReportExportFormat.Csv:
					extension = "csv";
					break;
				case ReportExportFormat.Pdf:
					extension = "pdf";
					break;
				case ReportExportFormat.Rtf:
					extension = "rtf";
					break;
				case ReportExportFormat.Xls:
					extension = "xls";
					break;
				case ReportExportFormat.Xlsx:
					extension = "xlsx";
					break;
				default:
					new ArgumentOutOfRangeException(nameof(format),Droid.Resources.Messages.Text_Error_Could_not_determine_file_format);
					break;
			}

			return extension;
		}
		/// <summary>
		/// Сохраняет ответ от сервера по генерации отчета в файл, указанного типа, и возвращает полное имя файла
		/// </summary>
		/// <param name="response"></param>
		/// <param name="exportFormat"></param>
		public static string SaveResponse(ExportedReport response, ReportExportFormat exportFormat)
		{
			string extension = ReportService.GetExtensionByFormat(exportFormat);

			string fileName = response.FileName;
			fileName = Lers.Utils.FileUtils.SanitizeFileName(fileName);

			string directoryName = StorageDirectoryService.Get();

			string fullName = Lers.Utils.FileUtils.CreateFullFileName(directoryName, fileName, extension);

			File.WriteAllBytes(fullName, response.Content);

			return fullName;
		}

		/// <summary>
		/// Получает наименование группы по перечислению типов
		/// </summary>
		/// <param name="groupType"></param>
		/// <returns></returns>
		internal static string GetReportGroupDescription(ReportGroupType groupType)
		{
			switch (groupType)
			{
				case ReportGroupType.Acts:
					return Droid.Resources.Messages.Text_ReportGroupType_Acts;
				case ReportGroupType.ParametersSheets:
					return Droid.Resources.Messages.Text_ReportGroupType_ParametersSheet;
				case ReportGroupType.Passports:
					return Droid.Resources.Messages.Text_ReportGroupType_Passports;
				case ReportGroupType.Calibration:
					return Droid.Resources.Messages.Text_ReportGroupType_Calibration;
				case ReportGroupType.NodeJob:
					return Droid.Resources.Messages.Text_ReportGroupType_NodeJob;
				case ReportGroupType.SystemState:
					return Droid.Resources.Messages.Text_ReportGroupType_SystemState;
			}

			return Droid.Resources.Messages.Text_Others;
		}

		/// <summary>
		/// Получить идентификатор группы по типу отчета
		/// </summary>
		/// <param name="isAct"></param>
		/// <param name="reportType"></param>
		/// <returns></returns>
		internal static ReportGroupType GetReportGroupType(bool isAct, ReportType reportType)
		{
			if (isAct)
			{
				return ReportGroupType.Acts;
			}
			switch (reportType)
			{
				case ReportType.ParametersSheet:
					return ReportGroupType.ParametersSheets;
				case ReportType.NodePassport:
					return ReportGroupType.Passports;
				case ReportType.Calibration:
					return ReportGroupType.Calibration;
				case ReportType.NodeJob:
					return ReportGroupType.NodeJob;
				case ReportType.SystemState:
					return ReportGroupType.SystemState;
			}

			return ReportGroupType.Others;
		}

		/// <summary>
		/// Реализует функциональность обработчика выбора пункта Отчеты в главном меню приложения
		/// </summary>
		public static async void MainMenuSelectedSystemReports()
		{
			var reportLoader = new SystemReportLoader();

			await reportLoader.Reload(false);

			((MainPage)App.Current.MainPage).Detail = new NavigationPage(new ReportsPage(reportLoader));
		}
	}
}
