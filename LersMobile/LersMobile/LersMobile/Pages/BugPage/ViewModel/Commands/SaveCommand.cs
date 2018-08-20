using System;
using System.Windows.Input;

namespace LersMobile.Pages.BugPage.ViewModel.Commands
{
	/// <summary>
	/// Реализация обрботчика сохранения отчета по ошибке
	/// </summary>
	public class SaveCommand : ICommand
	{
		private readonly BugViewModel _viewModel;

		public SaveCommand(BugViewModel viewModel)
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
			_viewModel.Save();
		}
	}
}
