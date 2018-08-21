using LersMobile.Services.BugReport;
using LersMobile.Services.Resource;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LersMobile.Core
{
    /// <summary>
    /// Параметры НС, используемые для вывода на экран.
    /// </summary>
    public class IncidentView : INotifyPropertyChanged
    {
        private readonly Lers.Diag.Incident incident;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Определяет что НС открыта.
        /// </summary>
        public bool IsActive => !this.incident.IsClosed;

        public string Description => this.incident.Description;

        public string ShortDescription => this.incident.ShortDescription;

        public string ObjectTitle => this.incident.ObjectTitle;

        /// <summary>
        /// Дата начала НС.
        /// </summary>
        public string StartDate => this.incident.StartDateTime.ToString("dd.MM.yyyy");

        public string ImportanceImageSource => ResourceService.IncidentImportanceImage(this.incident.Importance);

        public string StateImageSource => this.incident.IsClosed ? "Check_16.png" : string.Empty;

        /// <summary>
        /// Журнал нештатной ситуации.
        /// </summary>
        public ObservableCollection<Lers.Diag.IncidentLogRecord> Log { get; private set; } = new ObservableCollection<Lers.Diag.IncidentLogRecord>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="incident"></param>
        public IncidentView(Lers.Diag.Incident incident)
        {
            this.incident = incident ?? throw new ArgumentNullException(nameof(incident));
        }

        /// <summary>
        /// Загружает записи журнала НС.
        /// </summary>
        /// <returns></returns>
        public async Task LoadLog()
        {
			var incidentLog = await this.incident.GetLogAsync();

			foreach (var record in incidentLog)
			{
				this.Log.Add(record);
			}			
        }

        /// <summary>
        /// Закрывает нештатную ситуаци.
        /// </summary>
        /// <returns></returns>
        public async Task Close()
        {
			await App.Core.EnsureConnected();

			await incident.CloseAsync();

			OnPropertyChanged(nameof(IsActive));
			OnPropertyChanged(nameof(StateImageSource));			
        }

        private void OnPropertyChanged(string propertyName) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
