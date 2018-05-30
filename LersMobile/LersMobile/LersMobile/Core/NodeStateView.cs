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
        public string StateImageSource
        {
            get
            {
                switch (this.state)
                {
                    case Lers.Core.NodeState.None: return "State_Unknown.png";
                    case Lers.Core.NodeState.Error: return "State_Error.png";
                    case Lers.Core.NodeState.Normal: return "State_Normal.png";
                    case Lers.Core.NodeState.Warning: return "State_Warning.png";

                    default:
                        throw new NotSupportedException("Неизвестное состояние объекта " + this.state);
                }
            }
        }

		public Action Action { get;set; }
    }
}
