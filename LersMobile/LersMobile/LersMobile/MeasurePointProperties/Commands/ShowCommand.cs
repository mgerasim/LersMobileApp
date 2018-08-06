using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.MeasurePointProperties.Commands
{
    public class ShowCommand : ICommand
    {
        public ShowCommand(MeasurePointArchivePage_ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public MeasurePointArchivePage_ViewModel ViewModel { get; set; }

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
