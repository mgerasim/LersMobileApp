using LersMobile.Core.ReportLoader;
using LersMobile.Views;
using System;
using System.ComponentModel;
using LersMobile.Pages.ReportsPage.ViewModel.Commands;
using System.Threading.Tasks;

namespace LersMobile.Pages.ReportsPage.ViewModel
{
	/// <summary>
	/// Класс модели представления для взаимодействия с пользователем на странице отображения отчетов ReportsPage
	/// </summary>
    public class ReportsViewModel : INotifyPropertyChanged
    {

		#region Закрытые свойства
				
		/// <summary>
		/// Реализация интерфейса по загрузке отчетов
		/// </summary>
		private IReportLoader _reportLoader;

		#endregion

        #region Команды

		/// <summary>
		/// Команда обновления списка отчетов
		/// </summary>
        public RefreshCommand RefreshCommand { get; set; }

        #endregion

        #region Binding свойства

		/// <summary>
		/// Источник данных отчетов для отображения
		/// </summary>
        public ReportsView[] Reports => _reportLoader.GetReports().ToArray();

		/// <summary>
		/// Признак загрузки данных
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
		/// Выбранный отчет
		/// </summary>		
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
                    OnPropertyChanged(nameof(SelectedReport));
                    Navigate();
                }
            }
        }

        #endregion

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="page"></param>
		/// <param name="reportLoader"></param>
		public ReportsViewModel(IReportLoader reportLoader)
        {
            _reportLoader = reportLoader ?? throw new ArgumentNullException(nameof(reportLoader), Droid.Resources.Messages.Text_Exception_Empty_param);

            RefreshCommand = new RefreshCommand(this);          
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
		/// Обновдение списка отчетов
		/// </summary>
		/// <param name="isForce"></param>
		/// <returns></returns>
        public async Task Refresh(bool isForce = false)
        {
            try
            {
                IsBusy = true;

                await _reportLoader.Reload(isForce);
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
			await ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ReportPage.ReportPage( _reportLoader.GetEntitiesIds(), 
                _reportLoader.GetReportEntity(), 
                SelectedReport));
        }

        #endregion
    }
}
