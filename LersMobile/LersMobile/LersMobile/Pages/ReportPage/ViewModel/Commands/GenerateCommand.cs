using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.Pages.ReportPage.ViewModel.Commands
{
	/// <summary>
	/// Класс команды обработчика генерации обчета в виде выходного файла
	/// </summary>
    public class GenerateCommand : ICommand
    {
		/// <summary>
		/// Экземпляр класса модель представления
		/// </summary>
		ReportViewModel _viewModel;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="viewModel"></param>
		public GenerateCommand(ReportViewModel viewModel)
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
		/// Обработчик понажатию инструмента генерации отчета
		/// </summary>
		/// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            try
            {
                await _viewModel.Generate();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error, ex.Message, "OK");
            }

        }
    }
}
