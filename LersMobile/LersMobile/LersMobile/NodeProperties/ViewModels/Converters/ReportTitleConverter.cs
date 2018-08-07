using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace LersMobile.NodeProperties.ViewModels.Converters
{
    public class ReportTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Lers.Core.NodeReport nodeReport = (Lers.Core.NodeReport)value;
            if (nodeReport == null)
            {
                return string.Empty;
            }
            return nodeReport.Report.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
