﻿using System.ComponentModel;

namespace LersMobile.Incidents
{
    public enum PageMode
    {
        [Description("Новые")]
        NewOnly,

        [Description("За период")]
        Interval
    }
}
