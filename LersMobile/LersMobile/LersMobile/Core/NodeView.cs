using System;
using System.Collections.Generic;
using System.Text;
using Lers.Core;

namespace LersMobile.Core
{
    public class NodeView
    {
		public Node Node { get; private set; }

		public string Address => this.Node.Address;

		public string Title => this.Node.Title;

		public string ImageSource
		{
			get
			{
				switch (this.Node.State)
				{
					case NodeState.Error: return "node_red.png";
					case NodeState.Normal: return "node_green.png";
					case NodeState.Warning: return "node_orange.png";
					case NodeState.None: return "node_gray.png";

					default:
						throw new NotSupportedException(this.Node.State.ToString());
				}
			}
		}

		public string ServicemanName => this.Node.Serviceman?.Name;
		public string CustomerTitle => this.Node.Customer?.Title;

		public string State
		{
			get
			{
				switch (this.Node.State)
				{
					case NodeState.Error: return "Есть ошибки";
					case NodeState.None: return "Неизвестно";
					case NodeState.Normal: return "Норма";
					case NodeState.Warning: return "Есть предупреждения";
					default:
						throw new NotSupportedException("Неизвестное состояние " + this.Node.State);
				}
			}
		}

		internal NodeView(Node node)
		{
			this.Node = node ?? throw new ArgumentNullException(nameof(node));
		}
    }
}
