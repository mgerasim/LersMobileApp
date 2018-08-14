using LersMobile.Core;
using LersMobile.MeasurePointProperties.ViewModels;
using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.MeasurePointProperties
{
	/// <summary>
	/// Страница свойств точки учёта.
	/// Содержит вкладки с детальными парметрами.
	/// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurePointReportsPage : ContentPage
    {
		/// <summary>
		/// Точка учёта, данные которой отображаются.
		/// </summary>
		public MeasurePointView MeasurePoint { get; private set; }

        private MeasurePointReportsViewModel ViewModel;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="measurePoint"></param>
        public MeasurePointReportsPage(MeasurePointView measurePoint)
        {
			this.MeasurePoint = measurePoint ?? throw new ArgumentNullException(nameof(measurePoint));

            InitializeComponent();

            this.Title = Droid.Resources.Messages.Text_Reports;

            ViewModel = new MeasurePointReportsViewModel(this, MeasurePoint.MeasurePoint);

            this.BindingContext = ViewModel;
        }

        /// <summary>
        /// Вызывается при отображении страницы на экране.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.Load();
        }
    }
}