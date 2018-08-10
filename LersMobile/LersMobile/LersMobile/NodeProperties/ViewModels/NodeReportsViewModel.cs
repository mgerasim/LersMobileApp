﻿using Lers.Core;
using Lers.Reports;
using LersMobile.Entities;
using LersMobile.NodeProperties.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace LersMobile.NodeProperties.ViewModels
{
    public class NodeReportsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public NodeReportsViewModel(NodeReportsPage page, Node node)
        {
            Node = node;
            Page = page;
            RefreshCommand = new RefreshCommand(this);
            reports = new ReportEntityCollection();
        }

        public RefreshCommand RefreshCommand { get; set; }

        private Node Node = null;

        private NodeReportsPage Page = null;

        private bool isBusy;

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        private ReportEntityCollection reports;

        public Entities.ReportEntity[] Reports
        {
            get
            {
                return reports.ToArray();
            }
        }
        
        private Entities.ReportEntity _selectedReport;

        public Entities.ReportEntity SelectedReport
        {
            get
            {
                return _selectedReport;
            }
            set
            {
                _selectedReport = value;
                if (value != null)
                {
                    OnPropertyChanged("SelectedReport");
                    Navigate();
                }
            }
        }

        public async Task Load(bool isForce = false)
        {
            try
            {
                IsBusy = true;

                await App.Core.EnsureConnected();

                var requiredFlags = NodeInfoFlags.Reports;

                if (!Node.AvailableInfo.HasFlag(requiredFlags) || isForce == true)
                {
                    await Node.RefreshAsync(requiredFlags);
                    reports.Reload(Node.Reports);
                    OnPropertyChanged("Reports");
                }
            }
            catch(Exception ex)
            {
                IsBusy = false;
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error, ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void Navigate()
        {
        //    await Page.Navigation.PushAsync(new NodeReportPage(Node, SelectedReport));
        }

        public async Task Refresh()
        {
            await Load(true);
        }
    }
}
