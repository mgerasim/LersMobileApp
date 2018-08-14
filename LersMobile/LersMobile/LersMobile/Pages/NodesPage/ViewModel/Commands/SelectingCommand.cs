using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.Pages.NodesPage.ViewModel.Commands
{
    public class SelectingCommand : ICommand
    {
        NodesViewModel ViewModel;

        public SelectingCommand(NodesViewModel viewModel)
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
            ViewModel.Selecting();
        }
    }
}
