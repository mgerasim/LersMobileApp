using Lers.Core;
using Lers.Reports;
using LersMobile.Pages.SystemReportsPage.ViewModel.Commands;
using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace LersMobile.Pages.SystemReportsPage.ViewModel
{
    public class SystemReportsViewModel : INotifyPropertyChanged
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public SystemReportsViewModel(SystemReportsPage page)
        {
            reports = new List<ReportView>();
            RefreshCommand = new RefreshCommand(this);
            Page = page;
            Load();
        }

        #region Закрытые свойства

        SystemReportsPage Page;

        #endregion

        #region Закрытые методы

        private async void Load()
        {
            try
            {
                var reportManager = new ReportManager(App.Core.Server);

                var reportList = await reportManager.GetReportListAsync();

                foreach (var report in reportList)
                {
                    if (report.Type == ReportType.SystemState ||
                        report.Type == ReportType.NodeJob ||
                        report.Type == ReportType.Calibration)
                    {
                        ReportView item = new ReportView(report);

                        this.reports.Add(item);
                    }
                }

                OnPropertyChanged("Reports");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error_Load, ex.Message, "Ok");
            }
            
        }

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

        public RefreshCommand RefreshCommand { get; }

        #endregion

        #region Binding свойства

        private List<ReportView> reports;

        public ReportView[] Reports
        {
            get
            {
                return reports.ToArray();
            }
        }

        private ReportView selectedReport;

        public ReportView SelectedReport
        {
            get
            {
                return selectedReport;
            }
            set
            {
                selectedReport = value;
                if (value != null)
                {
                    OnPropertyChanged("SelectedReport");
                    Navigate();
                }
            }
        }

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

        #endregion

        #region Методы комманд

        public async void Navigate()
        {
            await Page.Navigation.PushAsync(new ReportPage.ReportPage(-1, ReportEntity.System, SelectedReport));
        }

        public void Refresh()
        {
            Load();

            IsBusy = false;
            OnPropertyChanged("Reports");
        }

        #endregion
    }
}
