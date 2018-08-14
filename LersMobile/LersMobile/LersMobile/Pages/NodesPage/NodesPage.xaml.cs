using LersMobile.Pages.NodesPage.ViewModel;
using Xamarin.Forms;

namespace LersMobile.Pages.NodesPage
{
	/// <summary>
	/// Список объектов учёта.
	/// </summary>
	public partial class NodesPage : ContentPage
	{
        private NodesViewModel ViewModel;

        private bool isLoaded = false;

		/// <summary>
		/// Конструктор.
		/// </summary>
		public NodesPage()
		{
			InitializeComponent();
            
            this.ViewModel = new NodesViewModel();
            this.BindingContext = this.ViewModel;

			this.Title = Droid.Resources.Messages.MainPage_MenuItem_NodeList;

		}
        
		/// <summary>
		/// Обратаывает появление страницы на экране.
		/// </summary>
		protected override async void OnAppearing()
		{
			base.OnAppearing();

            if (!isLoaded)
            {
                await ViewModel.ReloadNodeGroups();
                await ViewModel.Refresh();

                isLoaded = true;
            }
        }

        public void OnSearchBarTextChanged(object sender, TextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(args.NewTextValue))
            {
                ViewModel.Search();
            }
        }
    }
}
