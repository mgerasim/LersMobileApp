using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.Pages.ReportsPage.ViewModel.Commands
{
	/// <summary>
	/// Класс реализует функционаьность обработчика обновления отчетов на странице отчетов
	/// </summary>
    public class RefreshCommand : ICommand
    {
		/// <summary>
		/// Модель предстваления
		/// </summary>
        ReportsViewModel _viewModel;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="viewModel"></param>
        public RefreshCommand(ReportsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;
		/// <summary>
		/// Признак доступности обработчика
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
		/// <summary>
		/// Выполнение обработчика
		/// </summary>
		/// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            await _viewModel.Refresh(true);
        }
    }
}
