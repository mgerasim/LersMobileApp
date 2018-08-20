using Lers.Data;
using Lers.Utils;
using LersMobile.MeasurePointProperties.ViewModels;
using LersMobile.Views;
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
		/// <summary>
		/// Экземпляр класса модель представления
		/// </summary>
        private MeasurePointArchiveViewModel _viewModel;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="measurePoint"></param>
		public MeasurePointArchivePage(MeasurePointView measurePoint)
		{
			InitializeComponent();

            _viewModel = new MeasurePointArchiveViewModel(_containerData, _dataGrid, measurePoint.MeasurePoint);
            this.BindingContext = _viewModel;

            this.Title = Droid.Resources.Messages.MeasurePointArchivePage_Title;
		}		

        /// <summary>
        /// Вызывается при отображении на экране.
        /// </summary>
        protected override async void OnAppearing()
		{
			base.OnAppearing();

            await _viewModel.LoadData();

		}
	}
}