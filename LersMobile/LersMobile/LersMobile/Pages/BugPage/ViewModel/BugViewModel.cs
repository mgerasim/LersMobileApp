using Lers.Telemetry;
using LersMobile.Core;
using LersMobile.Pages.BugPage.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LersMobile.Pages.BugPage.ViewModel
{
	/// <summary>
	/// Модель представления для обработки исключение в виде отчета по ошибке
	/// </summary>
	public class BugViewModel : INotifyPropertyChanged
	{


		#region Закрытые свойства
		/// <summary>
		/// Исключение для обработки
		/// </summary>
		readonly Exception _exception;
		/// <summary>
		/// Заголовок отчета по ошибке
		/// </summary>
		readonly string _title;
		/// <summary>
		/// Описание ошибки
		/// </summary>
		readonly string _description;
		/// <summary>
		/// Пользовательское сообщение
		/// </summary>
		string _message;
		/// <summary>
		/// Электронный адрес
		/// </summary>
		string _email;

		#endregion

		#region Команды

		/// <summary>
		/// Команда сохранения отчета по ошибке
		/// </summary>
		readonly public SaveCommand SaveCommand;

		/// <summary>
		/// Команда отправка отчета по ошибке
		/// </summary>
		readonly public SendCommand SendCommand;

		#endregion
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="title"></param>
		/// <param name="description"></param>
		/// <param name="exception"></param>
		public BugViewModel(string title, string description, Exception exception)
		{
			_exception = exception;
			this._title = title;
			this._description = description;
			SaveCommand = new SaveCommand(this);
			SendCommand = new SendCommand(this);
		}

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


		#region Binding свойства

		public string Description => _description;

		public string Message
		{
			get => _message;
			set
			{
				_message = value;
				OnPropertyChanged(nameof(Message));
			}
		}

		public string Email
		{
			get => _email;
			set
			{
				_email = value;
				OnPropertyChanged(nameof(Email));
			}
		}

		#endregion

		#region Методы комманд

		/// <summary>
		/// Отправка отчета на сервер телеметрии
		/// </summary>
		public async void Send()
		{
			await ((MainPage)App.Current.MainPage).Detail.Navigation.PopAsync();
		}

		/// <summary>
		/// Сохранение отчета об ошибке на устройстве пользователя
		/// </summary>
		public async void Save()
		{
			await((MainPage)App.Current.MainPage).Detail.Navigation.PopAsync();
		}

		#endregion

		#region Открытые методы


		#endregion
	}
}
