using LersMobile.Core;
using LersMobile.Pages.NodesPage;
using LersMobile.Services.Report;
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
	enum pageMenuItem
	{
		pageNodeList = 0,
		pageIncidentList = 1,
		pageNotificationCenter = 2,
        pageReportsysList = 3,
		actionExit = 4
	}
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
					new MainPageMenuItem() { Id = (int)pageMenuItem.pageNodeList, Title = Droid.Resources.Messages.MainPage_MenuItem_NodeList, TargetType = typeof(NodesPage)},
                    new MainPageMenuItem() { Id = (int)pageMenuItem.pageIncidentList, Title = Droid.Resources.Messages.MainPage_MenuItem_IncidentList, TargetType = typeof(Incidents.IncidentListMainPage) },
                    new MainPageMenuItem() { Id = (int)pageMenuItem.pageNotificationCenter, Title = Droid.Resources.Messages.MainPage_MenuItem_NotificationList, TargetType = typeof(NotificationCenterPage)},
                    new MainPageMenuItem() { Id = (int)pageMenuItem.pageReportsysList, Title = Droid.Resources.Messages.Text_Reports, TargetAction = ReportService.MainMenuSelectedSystemReports },
					new MainPageMenuItem() { Id = (int)pageMenuItem.actionExit, Title = Droid.Resources.Messages.MainPage_MenuItem_Exit, TargetAction = App.Core.Logout, BeginGroup = true }
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