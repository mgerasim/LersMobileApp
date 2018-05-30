using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
    public class IncidentView
    {
        private readonly Lers.Diag.Incident incident;

        public string ShortDescription => this.incident.ShortDescription;

        public string ObjectTitle => this.incident.ObjectTitle;

        public string ImportanceImageSource => ResourceHelper.GetIncidentImportanceImage(this.incident.Importance);

        public IncidentView(Lers.Diag.Incident incident)
        {
            this.incident = incident ?? throw new ArgumentNullException(nameof(incident));
        }
    }
}
