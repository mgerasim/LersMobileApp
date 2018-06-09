namespace LersMobile.Core
{
	/// <summary>
	/// Состояние объекта учёта, пригодное для вывода на экран.
	/// </summary>
	public class NodeStateView
	{
        private readonly Lers.Core.NodeState state;

		/// <summary>
		/// Идентификатор состояния.
		/// </summary>
		public DetailedStateId Id { get; private set; }

        public NodeStateView(Lers.Core.NodeState state, DetailedStateId id = DetailedStateId.None)
        {
            this.state = state;
			this.Id = id;
        }

		public string Text { get; set; }

		/// <summary>
		/// Изображение, описывающее состояние объекта.
		/// </summary>
		public string StateImageSource => ResourceHelper.GetNodeStateImage(this.state);
    }
}
