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

namespace LersMobile.Pages.NodesPage.ViewModel
{
    public class NodesViewModel : INotifyPropertyChanged
    {
        public NodesViewModel()
        {
            nodeGroups = new List<NodeGroupView>();
            RefreshCommand = new RefreshCommand(this);
            SearchCommand = new SearchCommand(this);
            SelectingCommand = new SelectingCommand(this);
            ReportCommand = new ReportCommand(this);

            _nodes = new List<SelectableData<NodeView>>();

            ItemTappedCommand = new Command((object model) => {

                if (model != null && model is ItemTappedEventArgs)
                {
                    ((SelectableData<NodeView>)((ItemTappedEventArgs)model).Item).IsSelected = !((SelectableData<NodeView>)((ItemTappedEventArgs)model).Item).IsSelected;
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
            set
            {
                if (value != null)
                {
                    selectedNode = value;
                }
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

        public void Report()
        {
            IsSelecting = false;
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

        #endregion
    }
}
