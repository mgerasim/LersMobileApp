using LersMobile.Pages.SystemReportsPage.ViewModel;
using System;
using System.Windows.Input;

namespace LersMobile.Pages.SystemReportsPage.ViewModel.Commands
{
    public class RefreshCommand : ICommand
    {
        public RefreshCommand(SystemReportsViewModel viewModel)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        SystemReportsViewModel ViewModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            try
            {
                ViewModel.Refresh();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Droid.Resources.Messages.Text_Error, ex.Message, "OK");
            }

        }
    }
}
