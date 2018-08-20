using LersMobile.Services.Resource;

namespace LersMobile.Views
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
		public DetailedState Id { get; private set; }


        public MeasurePointStateView(Lers.Core.MeasurePointState state, DetailedState stateId = DetailedState.None)
        {
			this.Id = stateId;
			this.state = state;			
        }

		public string Text { get; set; }

		/// <summary>
		/// Изображение, описывающее состояние объекта.
		/// </summary>
		public string StateImageSource => ResourceService.MeasurePointStateImage(this.state);
    }
}
