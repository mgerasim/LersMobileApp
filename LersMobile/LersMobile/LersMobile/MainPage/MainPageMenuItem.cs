using System;

namespace LersMobile
{
	/// <summary>
	/// Описывает пункт меню главной навигации.
	/// </summary>
	public class MainPageMenuItem
	{
		public MainPageMenuItem() { }

		public int Id { get; set; }

		public string Title { get; set; }

		public Type TargetType { get; set; }

		/// <summary>
		/// Действие, которое нужно выполнить при выборе пункта меню.
		/// </summary>
		public Action TargetAction { get; set; }

		/// <summary>
		/// Пункт имеет разделитель.
		/// </summary>
		public bool BeginGroup { get; set; }
	}
}