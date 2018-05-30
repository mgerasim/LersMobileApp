using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Lers.Utils;

namespace LersMobile.Incidents
{
    /// <summary>
    /// Отображает список нештатных ситуаций.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentListPage : ContentPage
    {
        private readonly PageMode pageMode;

        public bool ShowDateRangeControls => this.pageMode == PageMode.Interval;

        public IncidentListPage(PageMode pageMode)
        {
            this.pageMode = pageMode;

            InitializeComponent();

            this.Title = pageMode.GetDescription();
        }
    }
}