using Lers.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace LersMobile.MeasurePointProperties.ViewModels.Converters
{
    public class ReportTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MeasurePointReport measurePointReport = (MeasurePointReport) value;
            if (measurePointReport == null)
            {
                return string.Empty;
            }
            return measurePointReport.Report.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
