using Lers.Telemetry;
using Lers.Telemetry.Channel;
using LersMobile.Core;
using LersMobile.Pages.BugPage.ViewModel.Commands;
using LersMobile.Services.Device;
using LersMobile.Services.PopupMessage;
using LersMobile.Services.Telemetry;
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
		public SaveCommand SaveCommand { get; }

		/// <summary>
		/// Команда отправка отчета по ошибке
		/// </summary>
		public SendCommand SendCommand { get; }

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
			var telemetryChannel = new InMemoryChannel(TelemetryServices.ServerEndPointAddress);

#if DEBUG
			telemetryChannel.SendingInterval = new TimeSpan(0, 0, 5);
#else
			telemetryChannel.SendingInterval = new TimeSpan(0, 10, 0);
#endif
			TelemetryClient telemetryClient = new TelemetryClient(telemetryChannel);

			telemetryClient.Context.License.Type = App.Core.Server.License.LicenseType.ToString();

			telemetryClient.Context.Token = DeviceService.GetIdentifier();
			telemetryClient.Context.Component.Name = "MobileApp";
			telemetryClient.Context.Component.Id = DeviceService.GetIdentifier();
			telemetryClient.Context.Component.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();

			telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.VersionString;
			telemetryClient.Context.Device.Language = System.Globalization.CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
			telemetryClient.Context.Device.OsArchitecture = DeviceService.GetProcessorArchitecture();
			telemetryClient.Context.Location.TimeZone = TimeZone.CurrentTimeZone.StandardName;

			var properties = new Dictionary<string, string>();

			properties["username"] = Email;
			properties["userdescription"] = Description;

			await telemetryClient.TrackException(this._exception, this._message, properties);

			PopupMessageService.ShowShort(Droid.Resources.Messages.Text_Report_successfully_sended);

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
