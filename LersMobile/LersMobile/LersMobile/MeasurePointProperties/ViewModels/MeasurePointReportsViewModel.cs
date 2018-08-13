using Lers.Core;
using LersMobile.Core;
using LersMobile.MeasurePointProperties.ViewModels.Commands;
using LersMobile.Views;
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

            RefreshCommand = new RefreshCommand(this);

            reports = new List<ReportViewCollectionGrouping>();
        }

        public RefreshCommand RefreshCommand { get; set; }

        MeasurePointReportsPage Page;

        MeasurePoint MeasurePoint;

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

        private List<ReportViewCollectionGrouping> reports;

        public ReportViewCollectionGrouping[] Reports
        {
            get
            {
                return reports.ToArray();
            }
        }

        private ReportView _selectedReport;

        public ReportView SelectedReport
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

                MeasurePointInfoFlags requiredFlags = MeasurePointInfoFlags.Reports;

                if (!MeasurePoint.AvailableInfo.HasFlag(requiredFlags) || isForce == true)
                {
                    await MeasurePoint.RefreshAsync(requiredFlags);
                    
                    ReportViewCollection reportEntities = new ReportViewCollection();
                    reportEntities.Reload(MeasurePoint.Reports);
                    reports = ReportUtils.BuildReportEntityCollectionGrouping(reportEntities);

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
            await Page.Navigation.PushAsync(new Pages.ReportPage.ReportPage(MeasurePoint.Id, Lers.Reports.ReportEntity.MeasurePoint, SelectedReport));
        }

        public async Task Refresh()
        {
            await Load(true);
        }
    }
}
