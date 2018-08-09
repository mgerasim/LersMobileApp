using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.MeasurePointProperties.ViewModels.Commands
{
    public class ShowCommand : ICommand
    {
        public ShowCommand(MeasurePointArchiveViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public MeasurePointArchiveViewModel ViewModel { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.ShowFilter();
        }
    }
}
