﻿using LersMobile.Views;
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
using LersMobile.NodeProperties;
using LersMobile.Core.ReportLoader;
using Lers.Core;
using LersMobile.Services.PopupMessage;

namespace LersMobile.Pages.NodesPage.ViewModel
{
	/// <summary>
	/// Класс модели представления 
	/// </summary>
    public class NodesViewModel : INotifyPropertyChanged
    {
        #region Закрытые свойства

        private List<NodeGroupView> nodeGroups;

        private List<SelectableData<NodeView>> _nodes;
        
        private bool _isRefreshing = false;

        private string _searchText;

        private NodeGroupView selectedGroup;

        private bool isSelecting = false;

        private SelectableData<NodeView> selectedNode;

        #endregion
		
		#region Команды

		public readonly ICommand ItemTappedCommand;

		public readonly SearchCommand SearchCommand;

		public readonly RefreshCommand RefreshCommand;

		public readonly SelectingCommand SelectingCommand;

		public readonly ReportCommand ReportCommand;

		public readonly ReportMeasurePointsCommand ReportMeasurePointsCommand;

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
            get => isSelecting;
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
        public NodeGroupView[] NodeGroups => nodeGroups.ToArray();
        
        /// <summary>
        /// Возвращает выбранную для отображеня группу объектов или 0Se если
        /// выбрано отображение объектов изо всех групп.
        /// </summary>
        public NodeGroupView SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }
        
        /// <summary>
        /// Выбранный объект учёта
        /// </summary>
        public SelectableData<NodeView> SelectedNode => selectedNode;
		
        #endregion

		/// <summary>
		/// Конструктор
		/// </summary>
        public NodesViewModel()
        {
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
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error,
                    Droid.Resources.Messages.NodeListPage_Error_Loaded + Environment.NewLine + exc.Message, "OK");
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

            if (isSelecting)
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

            var reportLoader = new ReportLoaderNodes(nodeViews);

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

            var reportLoader = new ReportLoaderNodesMeasurePoints(measurePointViews);

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
