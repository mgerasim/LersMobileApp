using Lers.Data;
using Lers.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Xamarin.Forms;
using LersMobile.Views;
using Lers.Core;
using LersMobile.Core.ReportLoader;
using LersMobile.Pages.ReportsPage;

namespace LersMobile.Core
{
    public static class ReportUtils
    {
        public static DeviceDataType[] DataTypes = new DeviceDataType[]
        {
            DeviceDataType.Day, DeviceDataType.Hour, DeviceDataType.Month
        };

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
            }

            if (string.IsNullOrEmpty(extension))
            {
                throw new Exception(Droid.Resources.Messages.Text_Error_Could_not_determine_file_format);
            }

            return extension;
        }

        public static void SaveResponse(ExportedReport response, ReportExportFormat exportFormat)
        {
            string extension = ReportUtils.GetExtensionByFormat(exportFormat);

            string fileName = response.FileName;
            fileName = Lers.Utils.FileUtils.SanitizeFileName(fileName);

            string directoryName = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + global::Android.OS.Environment.DirectoryDownloads;

            string fullName = Lers.Utils.FileUtils.CreateFullFileName(directoryName, fileName, extension);

            File.WriteAllBytes(fullName, response.Content);

            DependencyService.Get<IMessage>().Show(Droid.Resources.Messages.Text_Report_successfully_created,true);

            Device.OpenUri(new Uri(fullName));
        }

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


        public static List<ReportViewCollectionGrouping> BuildReportEntityCollectionGrouping(ReportViewCollection reportEntities)
        {
            List<ReportViewCollectionGrouping> reportsGrouping = new List<ReportViewCollectionGrouping>();

            foreach (ReportGroupType type in (ReportType[])Enum.GetValues(typeof(ReportGroupType)))
            {
                var list = reportEntities.Where(x => x.GroupType == type);

                if (list.Count() > 0)
                {
                    ReportViewCollectionGrouping item = new ReportViewCollectionGrouping(list.First());
                    foreach(var element in list)
                    {
                        item.Add(element);
                    }
                    reportsGrouping.Add(item);
                }
            }

            return reportsGrouping;
        }

        public static async void MainMenuSelectedSystemReports()
        {
            var reportLoader = new ReportLoaderSystem();

            await reportLoader.Reload(false);

            ((MainPage)App.Current.MainPage).Detail = new NavigationPage(new ReportsPage(reportLoader));
        }
    }
}
