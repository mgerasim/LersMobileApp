using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Lers;
using LersMobile.Services.Resource;
using Xamarin.Forms;

namespace LersMobile.Core
{
    public class NotificationView : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public Notification Notification { get; private set; }

		public Color BackgroundColor => this.Notification.IsRead ? Color.Default	: Color.LightSteelBlue;

		public FontAttributes FontAttribute => this.Notification.IsRead ? FontAttributes.None : FontAttributes.Bold;

		public string Message => this.Notification.Message;

		public string DateTime => this.Notification.DateTime.ToString("dd.MM.yyyy HH:mm:ss");

        public string ImportanceImageSource => ResourceService.ImportanceImage(this.Notification.Importance);

		internal NotificationView(Notification notification)
		{
			this.Notification = notification ?? throw new ArgumentNullException(nameof(notification));
		}

		internal async Task MarkAsReadAsync()
		{
			await this.Notification.MarkAsReadAsync();

			OnPropertyChanged(nameof(BackgroundColor));
			OnPropertyChanged(nameof(FontAttribute));
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
