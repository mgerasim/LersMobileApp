using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LersMobile.Core
{
    /// <summary>
    /// Параметры НС, используемые для вывода на экран.
    /// </summary>
    public class IncidentView
    {
        private readonly Lers.Diag.Incident incident;

        public string ShortDescription => this.incident.ShortDescription;

        public string ObjectTitle => this.incident.ObjectTitle;

        public string ImportanceImageSource => ResourceHelper.GetIncidentImportanceImage(this.incident.Importance);

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
    }
}
