using LersMobile.Core.ReportLoader;
using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using LersMobile.Pages.ReportsPage.ViewModel.Commands;
using System.Threading.Tasks;

namespace LersMobile.Pages.ReportsPage.ViewModel
{
    public class ReportsViewModel : INotifyPropertyChanged
    {
        public ReportsViewModel(IReportLoader reportLoader)
        {
            if (reportLoader == null)
            {
                throw new ArgumentNullException();
            }

            ReportLoader = reportLoader;

            RefreshCommand = new RefreshCommand(this);
        }

        #region Закрытые свойства
        
        private bool isBusy;

        private IReportLoader ReportLoader;

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

        public RefreshCommand RefreshCommand { get; set; }

        #endregion

        #region Binding свойства

        public ReportViewCollectionGrouping[] Reports
        {
            get
            {
                return ReportLoader.GetReports().ToArray();
            }
        }
        
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

        public async Task Refresh(bool isForce = false)
        {
            try
            {
                IsBusy = true;

                await ReportLoader.Reload(isForce);
                OnPropertyChanged(nameof(Reports));
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Открытые методы


        #endregion
    }
}
