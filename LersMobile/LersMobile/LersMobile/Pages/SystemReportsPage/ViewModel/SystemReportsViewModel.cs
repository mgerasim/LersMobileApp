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

        private void Load()
        {
            reports.Clear();
            foreach (var itemEnum in Enum.GetValues(typeof(SystemReport)))
            {
                int[] array = new int[] { (int)SystemReport.SystemState };

                SortedSet<int> setIds = new SortedSet<int>();
                setIds.Add((int)SystemReport.SystemState);

                int id = (int)itemEnum;

                if (!setIds.Contains(id)) continue;

                ReportType type = ReportType.SystemState;
                string title = GetEnumDescription((Enum)itemEnum);

                ReportView report = new ReportView(id, type, title);

                this.reports.Add(report);
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
