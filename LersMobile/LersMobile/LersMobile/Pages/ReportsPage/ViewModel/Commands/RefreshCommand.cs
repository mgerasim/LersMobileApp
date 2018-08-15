using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.Pages.ReportsPage.ViewModel.Commands
{
    public class RefreshCommand : ICommand
    {
        ReportsViewModel ViewModel;

        public RefreshCommand(ReportsViewModel viewModel)
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
            ViewModel.Refresh(true);
        }
    }
}
