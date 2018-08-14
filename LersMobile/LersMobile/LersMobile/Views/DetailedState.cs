using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Views
{
    /// <summary>
    /// Определяет идентификатор детального состояния точки или объекта.
    /// </summary>
    public enum DetailedState
    {
        /// <summary>
        /// Не задано.
        /// </summary>
        None,

        /// <summary>
        /// Есть нештатные ситуации.
        /// </summary>
        Incidents,

        /// <summary>
        /// Есть критические нештатные ситуации.
        /// </summary>
        CriticalIncidents
    }
}
