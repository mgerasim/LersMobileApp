﻿using LersMobile.Core.ReportLoader;
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
        public ReportsViewModel(ReportsPage page, IReportLoader reportLoader)
        {
            ReportLoader = reportLoader ?? throw new ArgumentNullException();

            RefreshCommand = new RefreshCommand(this);

            Page = page;            
        }

        #region Закрытые свойства

        private ReportsPage Page;

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

        public ReportsView[] Reports
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
            catch (Exception ex)
            {
                IsBusy = false;
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error_Load,
                    $"{Droid.Resources.Messages.IncidentDetailPage_Error_Load_Description}. {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Открытые методы

        public async void Navigate()
        {
            await Page.Navigation.PushAsync(new ReportPage.ReportPage( ReportLoader.GetEntitiesIds(), 
                ReportLoader.GetReportEntity(), 
                SelectedReport));
        }

        #endregion
    }
}
