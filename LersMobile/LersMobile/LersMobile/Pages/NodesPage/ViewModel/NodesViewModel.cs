using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using LersMobile.Pages.NodesPage.ViewModel.Commands;
using LersMobile.Core;
using System.Windows.Input;
using Xamarin.Forms;
using LersMobile.NodeProperties;
using LersMobile.Core.ReportLoader;
using Lers.Core;
using LersMobile.Services.PopupMessage;
using LersMobile.Services.BugReport;

namespace LersMobile.Pages.NodesPage.ViewModel
{
	/// <summary>
	/// Класс модели представления 
	/// </summary>
    public class NodesViewModel : INotifyPropertyChanged
    {
        #region Закрытые свойства

        private List<NodeGroupView> _nodeGroups;

        private List<SelectableData<NodeView>> _nodes;
        
        private bool _isRefreshing = false;

        private string _searchText;

        private NodeGroupView _selectedGroup;

        private bool _isSelecting = false;

        private SelectableData<NodeView> _selectedNode;

        #endregion
		
		#region Команды

		public ICommand ItemTappedCommand { get; }

		public SearchCommand SearchCommand { get; }

		public RefreshCommand RefreshCommand { get; }

		public SelectingCommand SelectingCommand { get; }

		public ReportCommand ReportCommand { get; }

		public ReportMeasurePointsCommand ReportMeasurePointsCommand { get; }

		#endregion

		#region Binding свойства

		/// <summary>
		/// Список отображаемых объектов учёта.
		/// </summary>
		public SelectableData<NodeView>[] Nodes
        {
            get
            {
                if (string.IsNullOrEmpty(this.SearchText))
                {
                    return _nodes.ToArray();
                }
                else
                {
                    var searchText = this.SearchText.ToLower();

                    return _nodes
                        .Where(x => x.Data.Title.ToLower().Contains(searchText)
                            || x.Data.Address.ToLower().Contains(searchText))
                        .ToArray();
                }
            }
        }

        /// <summary>
		/// Флаг указывает что идёт обновление данных.
		/// </summary>
		public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        /// <summary>
		/// Флаг указывает что идёт множественный выбор.
		/// </summary>
        public bool IsSelecting
        {
            get => _isSelecting;
            set
            {
                _isSelecting = value;
                OnPropertyChanged(nameof(IsSelecting));
            }
        }
        
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
                OnPropertyChanged(nameof(Nodes));
            }
        }


        /// <summary>
        /// Текст для поиска.
        /// </summary>
        public NodeGroupView[] NodeGroups => _nodeGroups.ToArray();
        
        /// <summary>
        /// Возвращает выбранную для отображеня группу объектов или 0Se если
        /// выбрано отображение объектов изо всех групп.
        /// </summary>
        public NodeGroupView SelectedGroup
        {
            get => _selectedGroup;
            set
            {
				if (value != null)
				{
					_selectedGroup = value;
					AppDataStorage.SelectedGroupId = value.Id;
					OnPropertyChanged(nameof(SelectedGroup));
				}
                
            }
        }
        
        /// <summary>
        /// Выбранный объект учёта
        /// </summary>
        public SelectableData<NodeView> SelectedNode => _selectedNode;
		
        #endregion

		/// <summary>
		/// Конструктор
		/// </summary>
        public NodesViewModel()
        {
 //           _nodeGroups = new List<NodeGroupView>();
            RefreshCommand = new RefreshCommand(this);
            SearchCommand = new SearchCommand(this);
            SelectingCommand = new SelectingCommand(this);
            ReportCommand = new ReportCommand(this);
            ReportMeasurePointsCommand = new ReportMeasurePointsCommand(this);			
            _nodes = new List<SelectableData<NodeView>>();
			
            ItemTappedCommand = new Command((object model) => {

                if (model != null && model is ItemTappedEventArgs)
                {
                    _selectedNode = ((SelectableData<NodeView>)((ItemTappedEventArgs)model).Item);
                    if (IsSelecting)
                    {
                        _selectedNode.IsSelected = !_selectedNode.IsSelected;
                    }
                    else
                    {
                        Navigate();
                    }                    
                }
            });
        }
		
		#region INotifyPropertyChanged implement interface

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (propertyName != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region Методы комманд

		/// <summary>
		/// Поиск по наименованию объекта учета 
		/// </summary>
		public void Search()
        {
            OnPropertyChanged(nameof(Nodes));
        }

		/// <summary>
		/// Обновление данных
		/// </summary>
		/// <returns></returns>
        public async Task Refresh()
        {
            this.IsRefreshing = true;

            try
            {
                _nodes.Clear();

                int? nodeGroupId = null;
                if (SelectedGroup.Id > 0)
                {
                    nodeGroupId = SelectedGroup.Id;
                }

                var nodes = await App.Core.GetNodeDetail(nodeGroupId);

                foreach(var node in nodes)
                {
                    SelectableData<NodeView> selectableData = new SelectableData<NodeView>();
                    selectableData.Data = node;
                    this._nodes.Add(selectableData);
                }
				
                OnPropertyChanged(nameof(Nodes));
            }
            catch (Exception exc)
            {
				BugReportService.HandleException(Droid.Resources.Messages.Text_Error,
                    Droid.Resources.Messages.NodeListPage_Error_Loaded + Environment.NewLine + exc.Message, exc);
            }
            finally
            {
                this.IsRefreshing = false;
                this.IsSelecting = false;
            }
        }

		/// <summary>
		/// Переход в режим множественного выбора
		/// </summary>
        public void Selecting()
        {
            IsSelecting = !IsSelecting;

            if (_isSelecting)
            {
				PopupMessageService.ShowLong(Droid.Resources.Messages.Text_Select_accounting_objects_and_click_generate_report);
            }

            foreach(var node in _nodes)
            {
                node.IsSelecting = IsSelecting;
            }
            OnPropertyChanged(nameof(Nodes));
        }

		/// <summary>
		/// Переход в список отчётов
		/// </summary>
        public async void Report()
        {
            var nodeViews = new List<NodeView>();
            nodeViews.AddRange(Nodes.Where(x => x.IsSelected == true).Select(x => x.Data));

            var reportLoader = new NodesReportLoader(nodeViews);

			await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ReportsPage.ReportsPage(reportLoader));
        }

		/// <summary>
		/// Переход в список точек учёта
		/// </summary>
		/// <returns></returns>
        public async Task ReportMeasurePoints()
        {
            var measurePointViews = new List<MeasurePointView>();

            foreach(var nodeView in Nodes.Where(x => x.IsSelected == true).Select(x => x.Data))
            {
                var requiredFlags = NodeInfoFlags.Systems;

                if (!nodeView.Node.AvailableInfo.HasFlag(requiredFlags))
                {
                    // Нужные данные ещё не загружены
                    await nodeView.Node.RefreshAsync(requiredFlags);
                }

                foreach (var measurePoint in nodeView.Node.Systems.GetAllMeasurePoints())
                {
                    measurePointViews.Add(new MeasurePointView(measurePoint));
                }
            }

            var reportLoader = new NodesMeasurePointsReportLoader(measurePointViews);

			await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ReportsPage.ReportsPage(reportLoader));
        }

        #endregion

        #region Открытые методы

		/// <summary>
		/// Загрузить список групп
		/// </summary>
		/// <returns></returns>
        public async Task ReloadNodeGroups()
        {
			try
			{
				if (this._nodeGroups.Count > 0)
				{
					return;
				}

				this._nodeGroups.Add(new NodeGroupView(0, Droid.Resources.Messages.Text_All));

				_selectedGroup = NodeGroups.First(); // Все

				var list = await App.Core.Server.NodeGroups.GetListAsync();

				foreach (var item in list)
				{
					NodeGroupView nodeGroupView = new NodeGroupView(item.Id, item.Title);
					_nodeGroups.Add(nodeGroupView);
					if (item.Id == AppDataStorage.SelectedGroupId)
					{
						_selectedGroup = nodeGroupView;
					}
				}

				OnPropertyChanged(nameof(SelectedGroup));
				OnPropertyChanged(nameof(NodeGroups));
			}
			catch (Exception exc)
			{
				BugReportService.HandleException(Droid.Resources.Messages.Text_Error,
					Droid.Resources.Messages.NodeListPage_Error_Loaded + Environment.NewLine + exc.Message, exc);
			}
		}
        
		/// <summary>
		/// Переход на страницу просмотра параметров объекта учета
		/// </summary>
        public async void Navigate()
        {
			await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new NodePropertyPage(SelectedNode.Data));
        }

        #endregion
    }
}
