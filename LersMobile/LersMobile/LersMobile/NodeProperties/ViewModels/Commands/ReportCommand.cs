using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.NodeProperties.ViewModels.Commands
{
    public class ReportCommand : ICommand
    {
        public ReportCommand(NodeReportViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        NodeReportViewModel ViewModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.GenerateReport();
        }
    }
}
