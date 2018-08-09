using Android.Widget;
using Lers.Data;
using Lers.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

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

            Toast.MakeText(Android.App.Application.Context, 
                Droid.Resources.Messages.Text_Report_successfully_created,
                ToastLength.Long).Show();

            Device.OpenUri(new Uri(fullName));

        }
    }
}
