using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
    /// <summary>
    /// Список НС на определённую дату.
    /// </summary>
    public class DayIncidentList : List<IncidentView>
    {
        public DayIncidentList(DateTime dateTime)
        {
            this.DateTime = dateTime.ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Нештатные ситуации.
        /// </summary>
        public List<IncidentView> Incidents => this;

        /// <summary>
        /// Дата, к которой относятся нештатки.
        /// </summary>
        public string DateTime { get; private set; }
    }
}
