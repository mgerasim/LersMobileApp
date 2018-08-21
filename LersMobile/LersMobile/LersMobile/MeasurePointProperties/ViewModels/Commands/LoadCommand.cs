using System;
using System.Windows.Input;

namespace LersMobile.MeasurePointProperties.ViewModels.Commands
{
	/// <summary>
	/// Класс обработчика загрузки архивных данных
	/// </summary>
    public class LoadCommand : ICommand
    {
		/// <summary>
		/// Класс модели представления
		/// </summary>
		private readonly MeasurePointArchiveViewModel _viewModel;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="viewModel"></param>
        public LoadCommand(MeasurePointArchiveViewModel viewModel)
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
        public async void Execute(object parameter)
        {
            await _viewModel.LoadData();
        }
    }
}
