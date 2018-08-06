using Lers.Data;
using Lers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
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

        private MeasurePointArchivePage_ViewModel ViewModel;

		public MeasurePointArchivePage(Core.MeasurePointView measurePoint)
		{
			InitializeComponent();

            ViewModel = new MeasurePointArchivePage_ViewModel(containerData, dataGrid, measurePoint.MeasurePoint);
            this.BindingContext = ViewModel;

            this.Title = Droid.Resources.Messages.MeasurePointArchivePage_Title;
		}		

        /// <summary>
        /// Вызывается при отображении на экране.
        /// </summary>
        protected override async void OnAppearing()
		{
			base.OnAppearing();

            ViewModel.LoadData();

		}
        
		private void Filter_ToolbarItem_Clicked()
		{
           
        }
	}
}