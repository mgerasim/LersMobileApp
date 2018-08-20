using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.Pages.NodesPage.ViewModel.Commands
{
	/// <summary>
	/// Команда отображения списка отчётов
	/// </summary>
    public class ReportCommand : ICommand
    {
		/// <summary>
		/// Экземпляр класса модели представления
		/// </summary>
        private readonly NodesViewModel _viewModel;

		/// <summary>
		/// Конструктор 
		/// </summary>
		/// <param name="viewModel"></param>
        public ReportCommand(NodesViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Признак того, что команда доступна
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

		/// <summary>
		/// Обработчик команды
		/// </summary>
		/// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _viewModel.Report();
        }
    }
}
