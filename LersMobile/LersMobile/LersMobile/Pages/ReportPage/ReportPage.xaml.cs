using Lers.Reports;
using LersMobile.Pages.ReportPage.ViewModel;
using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.Pages.ReportPage
{
	/// <summary>
	/// Класс страницы отображения фильтрации данных для генерации выбранного отчёта
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportPage : ContentPage
	{
		/// <summary>
		/// Экземпляр класса модели представления
		/// </summary>
        ReportViewModel _viewModel;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="entity"></param>
		/// <param name="report"></param>
		public ReportPage (int[] entityIds, ReportEntity entity, ReportView report)
		{
			InitializeComponent ();

            _viewModel = new ReportViewModel(entityIds, entity, report);
            this.BindingContext = _viewModel;

            Title = Droid.Resources.Messages.Text_Report;
        }
	}
}