using LersMobile.Services.BugReport;
using LersMobile.Views;
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
    /// Страница отображает список точек учёта объекта.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NodeMeasurePointsPage : ContentPage
    {
		/// <summary>
		/// Объект учёта, к которой принадлежат точки учёта
		/// </summary>
        private NodeView _node;

        public NodeView Node
        {
            get => _node;
            private set
            {
                _node = value;
                OnPropertyChanged(nameof(Node));
            }
        }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="nodeView"></param>
        public NodeMeasurePointsPage(NodeView nodeView)
        {
            InitializeComponent();

            this.BindingContext = this;

            this.Node = nodeView ?? throw new ArgumentNullException(nameof(nodeView));

			this.Title = Droid.Resources.Messages.NodeMeasurePointsPage_Title;
		}


		/// <summary>
		/// Пользователь щёлкнул на точке учёта объекта.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			try
			{
				var measurePoint = (MeasurePointView)e.SelectedItem;

				var listView = (ListView)sender;

				listView.SelectedItem = null;

				if (measurePoint == null)
				{
					return;
				}

				await this.Navigation.PushAsync(new MeasurePointProperties.MeasurePointPropertiesPage(measurePoint));
			}
			catch (Exception exc)
			{
				BugReportService.HandleException(Droid.Resources.Messages.Text_Error_Load, exc.Message, exc);
			}
		}
	}
}