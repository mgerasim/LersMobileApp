using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LersMobile
{
	/// <summary>
	/// Список объектов учёта.
	/// </summary>
	public partial class NodeListPage : ContentPage
	{
		private readonly Core.MobileCore lersService;

		private bool isLoaded = false;

		private Lers.LersServer server => this.lersService.Server;

		private Lers.Core.NodeGroup[] nodeGroupList;

		/// <summary>
		/// Возвращает выбранную для отображеня группу объектов или null если
		/// выбрано отображение объектов изо всех групп.
		/// </summary>
		private Lers.Core.NodeGroup SelectedGroup
		{
			get
			{
				if (this.nodeGroupList == null || this.nodeGroupPicker.SelectedIndex == 0)
				{
					// Список объектов пуст или выбран первый пункт (все объекты)

					return null;
				}

				// Возвращаем выбранный объект
				return this.nodeGroupList[this.nodeGroupPicker.SelectedIndex - 1];

			}
		}

		private Core.NodeView[] _nodes;

		/// <summary>
		/// Список отображаемых объектов учёта.
		/// </summary>
		public Core.NodeView[] Nodes
		{
			get
			{
				if (string.IsNullOrEmpty(this.SearchText))
				{
					return _nodes;
				}
				else
				{
					var searchText = this.SearchText.ToLower();

					return _nodes
						.Where(x => x.Title.ToLower().Contains(searchText)
							|| x.Address.ToLower().Contains(searchText))
						.ToArray();
				}
			}
			set
			{
				_nodes = value;
				OnPropertyChanged(nameof(Nodes));
			}
		}


		private bool _isRefreshing = false;

		/// <summary>
		/// Флаг указывает что идёт обновление данных.
		/// </summary>
		public bool IsRefreshing
		{
			get { return _isRefreshing; }
			set
			{
				_isRefreshing = value;
				OnPropertyChanged(nameof(IsRefreshing));
			}
		}

		private bool _isSearchVisible = false;

		public bool IsSearchVisible
		{
			get => _isSearchVisible;
			set
			{
				_isSearchVisible = value;
				OnPropertyChanged(nameof(IsSearchVisible));
			}
		}

		private string _searchText;

		/// <summary>
		/// Текст для поиска.
		/// </summary>
		public string SearchText
		{
			get => _searchText;
			set
			{
				_searchText = value;
				OnPropertyChanged(nameof(SearchText));
			}
		}


		public ICommand RefreshCommand => new Command(async () => await RefreshData());

		public ICommand SearchCommand => new Command(() => OnPropertyChanged(nameof(Nodes)));


		/// <summary>
		/// Конструктор.
		/// </summary>
		public NodeListPage()
		{
			lersService = App.Core;

			InitializeComponent();

			this.nodeListView.RefreshCommand = this.RefreshCommand;

			this.nodeListView.ItemSelected += NodeListView_ItemSelected;

			this.BindingContext = this;
		}

		/// <summary>
		/// Обрабатывает выбор элемента управления.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NodeListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var nodeDetail = (Core.NodeView)e.SelectedItem;

			this.nodeListView.SelectedItem = null;

			if (nodeDetail != null)
			{
				this.Navigation.PushAsync(new NodeProperties.NodePropertyPage(nodeDetail));
			}
		}

		/// <summary>
		/// Обратаывает появление страницы на экране.
		/// </summary>
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isLoaded)
			{
				return;
			}

			this.IsRefreshing = true;

			// Запрашиваем список объектов

			await LoadNodeGroupList();

			this.IsRefreshing = false;

			this.nodeGroupPicker.SelectedIndexChanged += NodeGroupPicker_SelectedIndexChanged;

			this.nodeListView.BeginRefresh();

			this.isLoaded = true;
		}

		/// <summary>
		/// Загружает список групп объектов учёта.
		/// </summary>
		/// <returns></returns>
		private async Task LoadNodeGroupList()
		{
			this.nodeGroupList = await this.lersService.Server.NodeGroups.GetListAsync();

			this.nodeGroupPicker.Items.Add("Все");

			foreach (var ng in this.nodeGroupList)
			{
				this.nodeGroupPicker.Items.Add(ng.ToString());
			}

			// Установим индекс выбранной для отображения группы объектов.

			int selectedIndex = GetStoredSelectedGroupIndex();

			this.nodeGroupPicker.SelectedIndex = selectedIndex;
		}

		private int GetStoredSelectedGroupIndex()
		{
			int selectedIndex = 0;

			int storedSelectedGroupId = AppDataStorage.SelectedGroupId;
			if (storedSelectedGroupId > 0)
			{
				var selectedGroup = this.nodeGroupList.FirstOrDefault(x => x.Id == storedSelectedGroupId);

				if (selectedGroup != null)
				{
					// Прибавим 1, чтобы получить индекс с учётом всех объектов.

					selectedIndex = Array.IndexOf(this.nodeGroupList, selectedGroup) + 1;
				}
			}

			return selectedIndex;
		}

		/// <summary>
		/// Загружает список объектов для выбранной группы.
		/// </summary>
		/// <returns></returns>
		private async Task RefreshData()
		{
			this.IsRefreshing = true;

			try
			{
				this.Nodes = await this.lersService.GetNodeDetail(this.SelectedGroup?.Id);
			}
			catch (Exception exc)
			{
				await DisplayAlert("Ошибка", "Не удалось загрузить список объектов." + Environment.NewLine + exc.Message, "OK");
			}
			finally
			{
				this.IsRefreshing = false;
			}
		}


		/// <summary>
		/// Обрабатывает изменение выбранной группы объектов учёта.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void NodeGroupPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.isLoaded || this.IsRefreshing)
			{
				return;
			}

			this.SearchText = string.Empty;

			await RefreshData();

			AppDataStorage.SelectedGroupId = this.SelectedGroup?.Id ?? -1;
		}

		/// <summary>
		/// Пользователь нажал на кнопку скрытия или отображения панели поиска.
		/// </summary>
		public void OnSearchToggle() => IsSearchVisible = !IsSearchVisible;

		public void OnSearchBarTextChanged(object sender, TextChangedEventArgs args)
		{
			if (string.IsNullOrEmpty(args.NewTextValue))
			{
				OnPropertyChanged(nameof(Nodes));
			}
		}
	}
}
