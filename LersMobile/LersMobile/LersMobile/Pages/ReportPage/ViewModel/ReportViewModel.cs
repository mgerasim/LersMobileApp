using Lers.Reports;
using LersMobile.Core;
using LersMobile.Pages.ReportPage.ViewModel.Commands;
using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace LersMobile.Pages.ReportPage.ViewModel
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        public ReportViewModel(int entityId, ReportEntity entity, ReportView report)
        {
            Entity = entity;
            Report = report;
            _dateBgn = DateTime.Now.AddDays(-7);
            _dateEnd = DateTime.Now;
            _isBusy = false;
            EntityId = entityId;
            GenerateCommand = new GenerateCommand(this);
        }

        #region Закрытые свойства

        private ReportEntity Entity;

        private ReportView Report;

        private int EntityId;

        # endregion

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

        public GenerateCommand GenerateCommand { get; protected set; }

        #endregion

        #region Binding свойства

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        private DateTime _dateBgn;

        public DateTime DateBgn
        {
            get
            {
                return _dateBgn;
            }
            set
            {
                _dateBgn = value;
                OnPropertyChanged("DateBgn");
            }
        }

        private DateTime _dateEnd;

        public DateTime DateEnd
        {
            get
            {
                return _dateEnd;
            }
            set
            {
                _dateEnd = value;
                OnPropertyChanged("DateEnd");
            }
        }

        private int _selectedDataType;

        public int SelectedDataType
        {
            get
            {
                return _selectedDataType;
            }
            set
            {
                _selectedDataType = value;
                OnPropertyChanged("SelectedDataType");
            }
        }
        private int _selectedFileFormat;

        public int SelectedFileFormat
        {
            get
            {
                return _selectedFileFormat;
            }
            set
            {
                _selectedFileFormat = value;
                OnPropertyChanged("SelectedFileFormat");
            }
        }

        #endregion

        #region Методы комманд

        public async Task Generate()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                var reportExportOptions = new ReportExportOptions();
                reportExportOptions.Format = (ReportExportFormat)SelectedFileFormat;
                var reportManager = new ReportManager(App.Core.Server);

                var response = await reportManager.GenerateExported(
                    reportExportOptions,
                    new int[] { EntityId },
                    null,
                    Entity,
                    Report.Type,
                    Report.Id,
                    ReportUtils.DataTypes[SelectedDataType],
                    DateBgn, DateEnd);

                IsBusy = false;

                ReportUtils.SaveResponse(response, reportExportOptions.Format);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error, ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion



    }
}
