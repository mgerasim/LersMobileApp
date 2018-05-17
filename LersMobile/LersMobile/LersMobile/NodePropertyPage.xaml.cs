using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Lers.Core;

namespace LersMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NodePropertyPage : ContentPage
	{
		private Node _node;

		public string Test => "Hello world";

		public Node Node
		{
			get
			{
				return _node;
			}
			private set
			{
				_node = value;
				OnPropertyChanged(nameof(Node));
			}
		}

		public NodePropertyPage(Node node)
		{
			InitializeComponent();

			this.BindingContext = this;

			this.Node = node ?? throw new ArgumentNullException(nameof(node));
		}
	}
}