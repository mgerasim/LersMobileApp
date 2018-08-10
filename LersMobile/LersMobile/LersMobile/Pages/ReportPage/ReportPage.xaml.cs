using Lers.Reports;
using LersMobile.Pages.ReportPage.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.Pages.ReportPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportPage : ContentPage
	{
        ReportViewModel ViewModel;

		public ReportPage (int entityId, ReportEntity entity, Entities.ReportEntity report)
		{
			InitializeComponent ();

            ViewModel = new ReportViewModel(entityId, entity, report);
            this.BindingContext = ViewModel;

            Title = Droid.Resources.Messages.Text_Report;
        }
	}
}