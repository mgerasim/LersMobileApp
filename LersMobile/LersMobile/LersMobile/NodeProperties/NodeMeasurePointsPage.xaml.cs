using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.NodeProperties
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NodeMeasurePointsPage : ContentPage
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


        public NodeMeasurePointsPage(Core.NodeView nodeView)
        {
            InitializeComponent();

            this.BindingContext = this;

            this.Node = nodeView ?? throw new ArgumentNullException(nameof(nodeView));
        }
    }
}