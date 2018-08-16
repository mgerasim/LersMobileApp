using LersMobile.Core.ReportLoader;
using LersMobile.Pages.ReportsPage.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.Pages.ReportsPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportsPage : ContentPage
	{
        ReportsViewModel ViewModel;

        bool isLoaded = false;

		public ReportsPage (IReportLoader reportLoader)
		{
            if (reportLoader == null)
            {
                throw new ArgumentNullException();
            }

			InitializeComponent ();

            Title = Droid.Resources.Messages.Text_Reports;

            ViewModel = new ReportsViewModel(this, reportLoader);
            this.BindingContext = ViewModel;
		}

        /// <summary>
		/// Обратаывает появление страницы на экране.
		/// </summary>
		protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!isLoaded)
            {
                await ViewModel.Refresh();

                isLoaded = true;
            }
        }
    }
}