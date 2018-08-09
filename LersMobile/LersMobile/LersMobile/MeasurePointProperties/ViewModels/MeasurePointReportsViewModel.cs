using Lers.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace LersMobile.MeasurePointProperties.ViewModels
{
    class MeasurePointReportsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MeasurePointReportsViewModel(MeasurePointReportsPage page, MeasurePoint measurePoint)
        {
            Page = page;

            MeasurePoint = measurePoint;
        }

        MeasurePointReportsPage Page;

        MeasurePoint MeasurePoint;

        public bool IsBusy { get; set; }

        public MeasurePointReportCollection Reports
        {
            get
            {
                return MeasurePoint.Reports;
            }
        }

        private MeasurePointReport _selectedReport;

        public MeasurePointReport SelectedReport
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

                MeasurePointInfoFlags requiredFlags = MeasurePointInfoFlags.Reports;

                if (!MeasurePoint.AvailableInfo.HasFlag(requiredFlags))
                {
                    await MeasurePoint.RefreshAsync(requiredFlags);
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
            await Page.Navigation.PushAsync(new MeasurePointReportPage(MeasurePoint, SelectedReport));
        }
    }
}
