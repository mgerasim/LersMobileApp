using LersMobile.Pages.NodesPage.ViewModel;
using Xamarin.Forms;

namespace LersMobile.Pages.NodesPage
{
	/// <summary>
	/// Список объектов учёта.
	/// </summary>
	public partial class NodesPage : ContentPage
	{
		/// <summary>
		/// Экземпляр класса модели представления
		/// </summary>
        private readonly NodesViewModel _viewModel;

		/// <summary>
		/// Признак того, что данные загруженны
		/// </summary>
        private bool isLoaded = false;

		/// <summary>
		/// Конструктор.
		/// </summary>
		public NodesPage()
		{
			InitializeComponent();
            
            this._viewModel = new NodesViewModel();
            this.BindingContext = this._viewModel;

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
                await _viewModel.ReloadNodeGroups();
                await _viewModel.Refresh();

                isLoaded = true;
            }
        }

    }
}
