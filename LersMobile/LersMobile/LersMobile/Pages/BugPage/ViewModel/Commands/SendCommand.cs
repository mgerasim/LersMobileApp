using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.Pages.BugPage.ViewModel.Commands
{
	public class SendCommand : ICommand
	{
		BugViewModel ViewModel;

		public SendCommand(BugViewModel viewModel)
		{
			ViewModel = viewModel;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			ViewModel.Send();
		}
	}
}
