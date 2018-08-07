using LersMobile.NodeProperties.ViewModels;
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
    public partial class NodeReportsPage : ContentPage
    {
        NodeReportsViewModel ViewModel = null;

        Core.NodeView NodeView = null;

        public NodeReportsPage(Core.NodeView nodeView)
        {
            InitializeComponent();

            this.Title = Droid.Resources.Messages.Text_Reports;

            this.NodeView = nodeView;

            ViewModel = new NodeReportsViewModel(this, this.NodeView.Node);
            this.BindingContext = ViewModel;
        }


        /// <summary>
        /// Вызывается при отображении страницы на экране.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.Load();
        }
    }
}