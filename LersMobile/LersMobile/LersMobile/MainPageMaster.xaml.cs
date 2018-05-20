using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPageMaster : ContentPage
	{
		public ListView ListView;

		public MainPageMaster()
		{
			InitializeComponent();

			BindingContext = new MainPageMasterViewModel();
			ListView = MenuItemsListView;
		}

		class MainPageMasterViewModel : INotifyPropertyChanged
		{
			public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }

			public MainPageMasterViewModel()
			{
				MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
				{
					new MainPageMenuItem() { Id = 0, Title = "Объекты учёта", TargetType = typeof(NodeListPage)},
					new MainPageMenuItem() { Id = 0, Title = "Выйти из учётной записи", TargetAction = App.Core.Logout }
				});
			}

			#region INotifyPropertyChanged Implementation
			public event PropertyChangedEventHandler PropertyChanged;
			void OnPropertyChanged([CallerMemberName] string propertyName = "")
			{
				if (PropertyChanged == null)
					return;

				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
			#endregion
		}
	}
}