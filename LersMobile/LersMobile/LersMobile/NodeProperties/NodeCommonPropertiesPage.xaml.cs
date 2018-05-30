using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.NodeProperties
{
    /// <summary>
    /// Станица, отображающая общие свойства объекта учёта.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NodeCommonPropertiesPage : ContentPage
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


        public NodeCommonPropertiesPage(Core.NodeView node)
        {
            InitializeComponent();

            this.BindingContext = this;

            this.Node = node ?? throw new ArgumentNullException(nameof(node));

            this.nodeStateListView.ItemSelected += NodeStateListView_ItemSelected;
        }

        private void NodeStateListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            this.nodeStateListView.SelectedItem = null;
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

            try
            {
                await this.Node.LoadDetail();

                // Обновляем свойства объекта.

                OnPropertyChanged(nameof(Node));

                this.isLoaded = true;

                this.IsBusy = false;
            }
            catch (Exception exc) when (exc is TimeoutException || exc is Lers.LersException)
            {
                await DisplayAlert("Ошибка", "Не удалось загрузить свойства объекта." + Environment.NewLine + exc.Message,
                    "OK");
            }
            catch (Exception exc) when (exc is Lers.NoConnectionException || exc is Lers.Networking.RequestDisconnectException)
            {
            }
        }
    }
}