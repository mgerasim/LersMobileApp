using LersMobile.Droid;
using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
	/// <summary>
	/// Параметры вывода на экран состояния точки учёта.
	/// </summary>
	public class MeasurePointStateView
	{
        private readonly Lers.Core.MeasurePointState state;

        public MeasurePointStateView(Lers.Core.MeasurePointState state)
        {
            this.state = state;
        }

		public string Text { get; set; }

		/// <summary>
		/// Изображение, описывающее состояние объекта.
		/// </summary>
		public string StateImageSource => ResourceHelper.GetMeasurePointStateImage(this.state);


		public Action Action { get;set; }
    }
}
