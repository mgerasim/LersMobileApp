using Lers.Reports;
using LersMobile.Core;
using LersMobile.Pages.ReportPage.ViewModel.Commands;
using LersMobile.Services.BugReport;
using LersMobile.Services.PopupMessage;
using LersMobile.Services.Report;
using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LersMobile.Pages.ReportPage.ViewModel
{
	/// <summary>
	/// Класс модели представления
	/// </summary>
    public class ReportViewModel : INotifyPropertyChanged
    {
        #region Закрытые свойства

		/// <summary>
		/// Тип сущности для которых генерируется отчёт
		/// </summary>
        private ReportEntity _entity;

		/// <summary>
		/// Экземпляр отчета для которого устанавливаются фильтрация данных
		/// </summary>
        private ReportView Report;

		/// <summary>
		/// Идентификаторы сущностей
		/// </summary>
        private int[] EntityIds;

        # endregion

        #region Команды

		/// <summary>
		/// Обработчик по нажатию генерации отчета
		/// </summary>
        public GenerateCommand GenerateCommand { get; }

        #endregion

        #region Binding свойства

		/// <summary>
		/// Признак того, что на данный момент идёт обновления данных
		/// </summary>
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

		/// <summary>
		/// Дата начало периода выборки архивных данных
		/// </summary>
        private DateTime _dateStart;

        public DateTime DateStart
        {
            get =>  _dateStart;
            set
            {
                _dateStart = value;
                OnPropertyChanged(nameof(DateStart));
            }
        }

		/// <summary>
		/// Дата завершения периода выборки архивных данных
		/// </summary>
        private DateTime _dateEnd;

        public DateTime DateEnd
        {
            get => _dateEnd;
            set
            {
                _dateEnd = value;
                OnPropertyChanged(nameof(DateEnd));
            }
        }

		/// <summary>
		/// Выбранный тип данных
		/// </summary>
        private int _selectedDataType;

        public int SelectedDataType
        {
            get =>  _selectedDataType;
            set
            {
                _selectedDataType = value;
                OnPropertyChanged(nameof(SelectedDataType));
            }
        }

		/// <summary>
		/// Выбранный формат выходного файла после генерации отчета
		/// </summary>
        private int _selectedFileFormat;

        public int SelectedFileFormat
        {
            get => _selectedFileFormat;
            set
            {
                _selectedFileFormat = value;
                OnPropertyChanged(nameof(SelectedFileFormat));
            }
        }

        #endregion

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="entityIds"></param>
		/// <param name="entity"></param>
		/// <param name="report"></param>
        public ReportViewModel(int[] entityIds, ReportEntity entity, ReportView report)
        {
            _entity = entity;
            Report = report;
            _dateStart = DateTime.Now.AddDays(-7);
            _dateEnd = DateTime.Now;
            _isBusy = false;
            EntityIds = entityIds;
            GenerateCommand = new GenerateCommand(this);
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
		/// Реализация функциональности по получению генерации данных в виде выходного файла
		/// </summary>
		/// <returns></returns>
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
                    EntityIds,
                    null,
                    _entity,
                    Report.Type,
                    Report.Id,
                    ReportService.DataTypes[SelectedDataType],
                    DateStart, DateEnd);

                IsBusy = false;

                var fullName = ReportService.SaveResponse(response, reportExportOptions.Format);
				Device.OpenUri(new Uri(fullName));
				
				PopupMessageService.ShowLong(Droid.Resources.Messages.Text_Report_successfully_created);
			}
            catch (Exception exc)
            {
                IsBusy = false;
				BugReportService.HandleException(Droid.Resources.Messages.Text_Error, exc.Message, exc);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
