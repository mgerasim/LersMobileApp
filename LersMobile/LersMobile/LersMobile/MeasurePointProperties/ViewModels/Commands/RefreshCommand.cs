using System;
using System.Windows.Input;

namespace LersMobile.MeasurePointProperties.ViewModels.Commands
{
    class RefreshCommand : ICommand
    {
        public RefreshCommand(MeasurePointReportsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        MeasurePointReportsViewModel ViewModel;

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
