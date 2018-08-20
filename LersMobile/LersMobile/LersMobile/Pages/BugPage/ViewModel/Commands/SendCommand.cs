using System;
using System.Windows.Input;

namespace LersMobile.Pages.BugPage.ViewModel.Commands
{
	/// <summary>
	/// Реализация обработчика отправки отчета об ошибке на сервер телеметрии
	/// </summary>
	public class SendCommand : ICommand
	{
		private readonly BugViewModel _viewModel;

		public SendCommand(BugViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_viewModel.Send();
		}
	}
}
