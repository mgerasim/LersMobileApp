using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using LersMobile.Pages.NodesPage.ViewModel.Commands;
using LersMobile.Core;
using System.Windows.Input;
using Xamarin.Forms;
using Android.Widget;
using LersMobile.NodeProperties;
using LersMobile.Core.ReportLoader;
using Lers.Core;

namespace LersMobile.Pages.NodesPage.ViewModel
{
    public class NodesViewModel : INotifyPropertyChanged
    {
        NodesPage Page;

        public NodesViewModel(NodesPage page)
        {
            Page = page;

            nodeGroups = new List<NodeGroupView>();
            RefreshCommand = new RefreshCommand(this);
            SearchCommand = new SearchCommand(this);
            SelectingCommand = new SelectingCommand(this);
            ReportCommand = new ReportCommand(this);
            ReportMeasurePointsCommand = new ReportMeasurePointsCommand(this);

            _nodes = new List<SelectableData<NodeView>>();

            ItemTappedCommand = new Command((object model) => {

                if (model != null && model is ItemTappedEventArgs)
                {
                    selectedNode = ((SelectableData<NodeView>)((ItemTappedEventArgs)model).Item);
                    if (IsSelecting)
                    {
                        selectedNode.IsSelected = !selectedNode.IsSelected;
                    }
                    else
                    {
                        Navigate();
                    }                    
                }
            });
        }

        #region Закрытые свойства

        private List<NodeGroupView> nodeGroups;

        private List<SelectableData<NodeView>> _nodes;
        
        private bool _isRefreshing = false;

        private string _searchText;

        private NodeGroupView selectedGroup;

        private bool isSelecting = false;

        private SelectableData<NodeView> selectedNode;

        #endregion

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

        #region Команды

        public ICommand ItemTappedCommand { get; protected set; }

        public SearchCommand SearchCommand { get; set; }

        public RefreshCommand RefreshCommand { get; set; }

        public SelectingCommand SelectingCommand { get; set; }

        public ReportCommand ReportCommand { get; set; }

        public ReportMeasurePointsCommand ReportMeasurePointsCommand { get; set; }

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
            get { return _isRefreshing; }
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
            get
            {
                return isSelecting;
            }
            set
            {
                isSelecting = value;
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
        public NodeGroupView[] NodeGroups
        {
            get
            {
                return nodeGroups.ToArray();
            }
        }
        
        /// <summary>
        /// Возвращает выбранную для отображеня группу объектов или 0Se если
        /// выбрано отображение объектов изо всех групп.
        /// </summary>
        public NodeGroupView SelectedGroup
        {
            get
            {
                return selectedGroup;
            }
            set
            {
                selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }
        
        /// <summary>
        /// Выбранный объект учёта
        /// </summary>
        public SelectableData<NodeView> SelectedNode
        {
            get
            {
                return selectedNode;
            }
        }
        #endregion

        #region Методы комманд

        public void Search()
        {
            OnPropertyChanged(nameof(Nodes));
        }

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
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error,
                    Droid.Resources.Messages.NodeListPage_Error_Loaded + Environment.NewLine + exc.Message, "OK");
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }

        public void Selecting()
        {
            IsSelecting = !IsSelecting;

            if (isSelecting)
            {
                Toast.MakeText(Android.App.Application.Context,
                Droid.Resources.Messages.Text_Select_accounting_objects_and_click_generate_report,
                ToastLength.Long).Show();
            }

            foreach(var node in _nodes)
            {
                node.IsSelecting = IsSelecting;
            }
            OnPropertyChanged(nameof(Nodes));
        }

        public async void Report()
        {
            var nodeViews = new List<NodeView>();
            nodeViews.AddRange(Nodes.Where(x => x.IsSelected == true).Select(x => x.Data));

            var reportLoader = new ReportLoaderNodes(nodeViews);

            await Page.Navigation.PushAsync(new ReportsPage.ReportsPage(reportLoader));
        }

        public async void ReportMeasurePoints()
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

            var reportLoader = new ReportLoaderNodesMeasurePoints(measurePointViews);

            await Page.Navigation.PushAsync(new ReportsPage.ReportsPage(reportLoader));
        }

        #endregion

        #region Открытые методы

        public async Task ReloadNodeGroups()
        {
            if (this.nodeGroups.Count > 0)
            {
                return;
            }            

            this.nodeGroups.Add(new NodeGroupView(0, Droid.Resources.Messages.Text_All));

            var list = await App.Core.Server.NodeGroups.GetListAsync();

            foreach(var item in list)
            {
                NodeGroupView nodeGroupView = new NodeGroupView(item.Id, item.Title);
                nodeGroups.Add(nodeGroupView);
            }

            OnPropertyChanged(nameof(NodeGroups));

            SelectedGroup = NodeGroups.First(); // Все
        }

        public async void Navigate()
        {
            await Page.Navigation.PushAsync(new NodePropertyPage(SelectedNode.Data));
        }

        #endregion
    }
}
