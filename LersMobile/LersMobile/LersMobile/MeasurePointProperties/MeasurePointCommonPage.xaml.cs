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
	/// Общие свойства точки учёта.
	/// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurePointCommonPage : ContentPage
    {
		public MeasurePointView MeasurePoint { get; private set; }

        public MeasurePointCommonPage(MeasurePointView measurePointView)
        {
			this.MeasurePoint = measurePointView ?? throw new NullReferenceException(nameof(measurePointView));

            InitializeComponent();

			this.BindingContext = this;
        }
    }
}