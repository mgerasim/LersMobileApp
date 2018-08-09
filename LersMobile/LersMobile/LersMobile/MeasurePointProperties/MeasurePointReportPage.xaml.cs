using Lers.Core;
using LersMobile.MeasurePointProperties.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.MeasurePointProperties
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MeasurePointReportPage : ContentPage
	{
        private MeasurePoint MeasurePoint;

        private MeasurePointReport MeasurePointReport;

        private MeasurePointReportViewModel ViewModel;

		public MeasurePointReportPage(MeasurePoint measurePoint, MeasurePointReport measurePointReport)
		{
			InitializeComponent ();

            MeasurePoint = measurePoint;

            MeasurePointReport = measurePointReport;

            Title = Droid.Resources.Messages.Text_Report;

            ViewModel = new MeasurePointReportViewModel(MeasurePoint, MeasurePointReport);
            BindingContext = ViewModel;
        }
	}
}