using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using LersMobile.Pages.NodesPage.ViewModel.Commands;

namespace LersMobile.Pages.NodesPage.ViewModel
{
    public class NodesViewModel : INotifyPropertyChanged
    {
        public NodesViewModel()
        {
            nodeGroups = new List<NodeGroupView>();
            RefreshCommand = new RefreshCommand(this);
            SearchCommand = new SearchCommand(this);
        }

        #region Закрытые свойства

        private List<NodeGroupView> nodeGroups;

        private NodeView[] _nodes;
        
        private bool _isRefreshing = false;

        private string _searchText;

        private NodeGroupView selectedGroup;
        
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

        public SearchCommand SearchCommand { get; set; }

        public RefreshCommand RefreshCommand { get; set; }

        #endregion

        #region Binding свойства

        /// <summary>
		/// Список отображаемых объектов учёта.
		/// </summary>
		public NodeView[] Nodes
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
                int? nodeGroupId = null;
                if (SelectedGroup.Id > 0)
                {
                    nodeGroupId = SelectedGroup.Id;
                }

                this.Nodes = await App.Core.GetNodeDetail(nodeGroupId);
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
