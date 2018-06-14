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
	public partial class MeasurePointArchivePage : ContentPage
	{
		private Core.MeasurePointView _measurePoint;

		public MeasurePointArchivePage(Core.MeasurePointView measurePoint)
		{
			InitializeComponent();

			_measurePoint = measurePoint;

			this.BindingContext = this;
		}
	}
}