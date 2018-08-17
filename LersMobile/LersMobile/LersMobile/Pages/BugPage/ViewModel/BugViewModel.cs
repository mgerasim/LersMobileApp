using Lers.Telemetry;
using LersMobile.Core;
using LersMobile.Pages.BugPage.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LersMobile.Pages.BugPage.ViewModel
{
	public class BugViewModel : INotifyPropertyChanged
	{
		public BugViewModel(string title, string description, Exception exception)
		{
			Exception = exception;
			this.title = title;
			this.description = description;
			SaveCommand = new SaveCommand(this);
			SendCommand = new SendCommand(this);
		}

		#region Закрытые свойства

		Exception Exception;
		readonly string title;
		readonly string description;
		string message;
		string email;

		#endregion

		#region INotifyPropertyChanged implement interface

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			if (propertyName != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region Команды

		public SaveCommand SaveCommand { get; set; }
		public SendCommand SendCommand { get; set; }

		#endregion

		#region Binding свойства

		public string Description
		{
			get
			{
				return description;
			}
		}

		public string Message
		{
			get
			{
				return message;
			}
			set
			{
				message = value;
				OnPropertyChanged(nameof(Message));
			}
		}

		public string Email
		{
			get
			{
				return email;
			}
			set
			{
				email = value;
				OnPropertyChanged(nameof(Email));
			}
		}

		#endregion

		#region Методы комманд

		public async void Send()
		{
			await ((MainPage)App.Current.MainPage).Detail.Navigation.PopAsync();
		}

		public async void Save()
		{
			await((MainPage)App.Current.MainPage).Detail.Navigation.PopAsync();
		}

		#endregion

		#region Открытые методы


		#endregion
	}
}
