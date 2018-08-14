using LersMobile.Core;
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
    public partial class MeasurePointPropertiesPage : TabbedPage
    {
		/// <summary>
		/// Точка учёта, данные которой отображаются.
		/// </summary>
		public MeasurePointView MeasurePoint { get; private set; }

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="measurePoint"></param>
        public MeasurePointPropertiesPage(MeasurePointView measurePoint)
        {
			this.MeasurePoint = measurePoint ?? throw new ArgumentNullException(nameof(measurePoint));

            InitializeComponent();

			this.Children.Add(new MeasurePointCommonPage(measurePoint));
			this.Children.Add(new MeasurePointDataPage(measurePoint));
			this.Children.Add(new MeasurePointArchivePage(measurePoint));
            this.Children.Add(new MeasurePointReportsPage(measurePoint));

			this.BindingContext = this;
        }
    }
}