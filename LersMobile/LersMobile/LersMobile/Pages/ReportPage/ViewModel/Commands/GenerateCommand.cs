using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.Pages.ReportPage.ViewModel.Commands
{
    public class GenerateCommand : ICommand
    {
        public GenerateCommand(ReportViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        ReportViewModel ViewModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            try
            {
                await ViewModel.Generate();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error, ex.Message, "OK");
            }

        }
    }
}
