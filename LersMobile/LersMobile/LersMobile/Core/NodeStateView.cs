using LersMobile.Droid;
using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Core
{
	public class NodeStateView
	{
        private readonly Lers.Core.NodeState state;

        public NodeStateView(Lers.Core.NodeState state)
        {
            this.state = state;
        }

		public string Text { get; set; }

		/// <summary>
		/// Изображение, описывающее состояние объекта.
		/// </summary>
		public string StateImageSource => ResourceHelper.GetNodeStateImage(this.state);        

		public Action Action { get;set; }
    }
}
