using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Lers.Core;
using System.Collections.ObjectModel;

namespace LersMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NodePropertyPage : ContentPage
	{
		private bool isLoaded = false;

		private Core.NodeView _node;

		public Core.NodeView Node
		{
			get { return _node; }
			private set
			{
				_node = value;
				OnPropertyChanged(nameof(Node));
			}
		}

        /// <summary>
        /// Указывает что по объекту учёта есть детальное состояние (диагностическая карточка).
        /// </summary>
        public bool HasDetailedState => this.Node?.HasDetailedState == true;

    
		public NodePropertyPage(Core.NodeView node)
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

			if (this.isLoaded)
			{
				// Данные уже загружались.
				return;
			}

			this.IsBusy = true;

            await this.Node.LoadDetail();

            // Обновляем свойства объекта.

            OnPropertyChanged(nameof(Node));

            this.isLoaded = true;

			this.IsBusy = false;
		}
	}
}