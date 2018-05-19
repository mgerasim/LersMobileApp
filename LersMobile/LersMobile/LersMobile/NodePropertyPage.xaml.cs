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
		private bool isRefreshed = false;

		private Node _node;

		public Node Node
		{
			get { return _node; }
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

		
		/// <summary>
		/// Вызывается при отображении страницы на экране.
		/// </summary>
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isRefreshed)
			{
				// Данные уже загружались.
				return;
			}

			var requiredFlags = NodeInfoFlags.Serviceman | NodeInfoFlags.Customer;

			if (this.Node.AvailableInfo.HasFlag(requiredFlags))
			{
				// Нужные данные уже загружены.

				return;
			}

			this.IsBusy = true;

			var node = this.Node;

			await node.RefreshAsync(requiredFlags);

			this.isRefreshed = true;

			// Обновляем свойства объекта.

			this.Node = node;

			this.IsBusy = false;
		}
	}
}