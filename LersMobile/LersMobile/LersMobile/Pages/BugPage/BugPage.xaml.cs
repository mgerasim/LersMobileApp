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
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BugPage : ContentPage
	{
		BugViewModel ViewModel;

		public BugPage (string title, string description, Exception exception)
		{
			InitializeComponent ();

			ViewModel = new BugViewModel(title, description, exception);
			this.BindingContext = ViewModel;

			Title = Droid.Resources.Messages.BugPage_Title;
		}
	}
}