namespace LersMobile.Services.Report
{
	/// <summary>
	/// Перечисление по которым выполняется группировка отчетов для отображения на странице ReportsPage
	/// </summary>
	public enum ReportGroupType
	{
		/// <summary>
		/// Ведомость параметров
		/// </summary>
		ParametersSheets = 0,
		/// <summary>
		/// Акты
		/// </summary>
		Acts = 1,
		/// <summary>
		/// Паспорта объектов
		/// </summary>
		Passports = 2,
		/// <summary>
		/// Состояние системы
		/// </summary>
		SystemState = 3,
		/// <summary>
		/// Проводимые работы
		/// </summary>
		NodeJob = 4,
		/// <summary>
		/// Проверка
		/// </summary>
		Calibration = 5,
		/// <summary>
		/// Другие
		/// </summary>
		Others = 6
	}
}
