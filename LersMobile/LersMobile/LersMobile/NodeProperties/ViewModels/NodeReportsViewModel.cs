using Lers.Core;
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
        }

        private Node Node = null;

        private NodeReportsPage Page = null;

        public bool IsBusy { get; set; }
                
        public Lers.Core.NodeReportCollection Reports
        {
            get
            {
                return Node.Reports;
            }
        }

        private NodeReport _selectedReport;

        public NodeReport SelectedReport
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

        public async Task Load()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                await App.Core.EnsureConnected();

                var requiredFlags = NodeInfoFlags.Reports;

                if (!Node.AvailableInfo.HasFlag(requiredFlags))
                {
                    await Node.RefreshAsync(requiredFlags);
                    OnPropertyChanged("Reports");

                }
            }
            catch
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void Navigate()
        {
            await Page.Navigation.PushAsync(new NodeReportPage(SelectedReport));
        }

    }
}
