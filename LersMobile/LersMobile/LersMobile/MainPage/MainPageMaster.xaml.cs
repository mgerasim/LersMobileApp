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
	/// <summary>
	/// Главное меню приложения.
	/// </summary>
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
					new MainPageMenuItem() { Id = 0, Title = LersMobile.Droid.Resources.Messages.NodeList, TargetType = typeof(NodeListPage)},
                    new MainPageMenuItem() { Id = 1, Title = LersMobile.Droid.Resources.Messages.IncidentList, TargetType = typeof(Incidents.IncidentListMainPage) },
                    new MainPageMenuItem() { Id = 2, Title = LersMobile.Droid.Resources.Messages.Notifications, TargetType = typeof(NotificationCenterPage)},
					new MainPageMenuItem() { Id = 3, Title = LersMobile.Droid.Resources.Messages.Exit, TargetAction = App.Core.Logout, BeginGroup = true }
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