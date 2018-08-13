using LersMobile.Pages.SystemReportsPage.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.Pages.SystemReportsPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SystemReportsPage : ContentPage
	{
        SystemReportsViewModel ViewModel;

		public SystemReportsPage ()
		{
			InitializeComponent ();
                        
            ViewModel = new SystemReportsViewModel(this);
            this.BindingContext = ViewModel;
		}
	}
}