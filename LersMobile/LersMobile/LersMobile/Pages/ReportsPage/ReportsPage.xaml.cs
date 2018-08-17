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
	/// <summary>
	/// Класс страницы для отображение отчетов
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportsPage : ContentPage
	{
		/// <summary>
		/// Модель представления для взаимодействия с пользователем на странице отображения отчетов
		/// </summary>
        ReportsViewModel _viewModel;

		/// <summary>
		/// Признак того, что отчеты загружены 
		/// </summary>
        bool isLoaded = false;

		/// <summary>
		/// Конструктор, принимающие интерфейс загрузки отчетов
		/// </summary>
		/// <param name="reportLoader"></param>
		public ReportsPage (IReportLoader reportLoader)
		{
			InitializeComponent ();

            Title = Droid.Resources.Messages.Text_Reports;

            _viewModel = new ReportsViewModel(this, reportLoader);
            this.BindingContext = _viewModel;
		}

        /// <summary>
		/// Обратаывает появление страницы на экране.
		/// </summary>
		protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!isLoaded)
            {
                await _viewModel.Refresh();

                isLoaded = true;
            }
        }
    }
}