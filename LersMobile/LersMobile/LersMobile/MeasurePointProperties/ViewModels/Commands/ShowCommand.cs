using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.MeasurePointProperties.ViewModels.Commands
{
	/// <summary>
	/// Класс обработчика показать панель фильтрации
	/// </summary>
    public class ShowCommand : ICommand
    {
		/// <summary>
		/// Экземпляр класса модели представления
		/// </summary>
		private readonly MeasurePointArchiveViewModel _viewModel;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="viewModel"></param>
        public ShowCommand(MeasurePointArchiveViewModel viewModel)
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
		/// Обработчик команды отображения панели фильтрации
		/// </summary>
		/// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _viewModel.ShowFilter();
        }
    }
}
