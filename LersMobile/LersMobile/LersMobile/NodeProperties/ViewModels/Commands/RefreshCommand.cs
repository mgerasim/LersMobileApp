using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LersMobile.NodeProperties.ViewModels.Commands
{
    public class RefreshCommand : ICommand
    {
        public RefreshCommand(NodeReportsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        NodeReportsViewModel ViewModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            try
            {
                await ViewModel.Refresh();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error, ex.Message, "OK");
            }

        }
    }
}
