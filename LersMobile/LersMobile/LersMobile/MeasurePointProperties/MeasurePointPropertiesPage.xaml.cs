using LersMobile.Core;
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
		public MeasurePointView MeasurePoint { get; private set; }

        public MeasurePointPropertiesPage(MeasurePointView measurePoint)
        {
			this.MeasurePoint = measurePoint ?? throw new ArgumentNullException(nameof(measurePoint));

            InitializeComponent();

			this.Children.Add(new MeasurePointCommonPage(measurePoint));

			this.BindingContext = this;
        }
    }
}