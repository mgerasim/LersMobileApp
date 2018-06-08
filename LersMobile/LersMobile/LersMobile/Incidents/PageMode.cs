using System.ComponentModel;

namespace LersMobile.Incidents
{
    public enum PageMode
    {
        [Description("Новые")]
        NewOnly,

        [Description("За период")]
        Interval,

		/// <summary>
		/// Активные НС по объекту или точке.
		/// </summary>
		[Description("Активные НС")]
		ObjectActive
    }
}
