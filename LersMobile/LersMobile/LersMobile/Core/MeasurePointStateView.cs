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
		
		/// <summary>
		/// Идентификатор состояния.
		/// </summary>
		public DetailedStateId Id { get; private set; }


        public MeasurePointStateView(Lers.Core.MeasurePointState state, DetailedStateId stateId = DetailedStateId.None)
        {
			this.Id = stateId;
			this.state = state;			
        }

		public string Text { get; set; }

		/// <summary>
		/// Изображение, описывающее состояние объекта.
		/// </summary>
		public string StateImageSource => ResourceHelper.GetMeasurePointStateImage(this.state);
    }
}
