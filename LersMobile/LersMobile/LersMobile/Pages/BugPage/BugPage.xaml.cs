using LersMobile.Pages.BugPage.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.Pages.BugPage
{
	/// <summary>
	/// Страница отчета по ошибке
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BugPage : ContentPage
	{
		private readonly BugViewModel _viewModel;

		public BugPage (string title, string description, Exception exception)
		{
			InitializeComponent ();

			_viewModel = new BugViewModel(title, description, exception);
			this.BindingContext = _viewModel;

			Title = Droid.Resources.Messages.BugPage_Title;
		}
	}
}