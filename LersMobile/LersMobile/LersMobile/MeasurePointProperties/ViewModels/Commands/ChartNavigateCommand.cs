using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.MeasurePointProperties.Commands
{
    public class ChartNavigateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        MeasurePointArchivePage_ViewModel ViewModel = null;

        public ChartNavigateCommand(MeasurePointArchivePage_ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            
        }
    }
}
