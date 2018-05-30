using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.Incidents
{
    /// <summary>
    /// Главная страница со списком НС. Позволяет отображать 
    /// новые нештатки или нештатки за указанный период.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentListMainPage : TabbedPage
    {
        public IncidentListMainPage()
        {
            InitializeComponent();

            this.Children.Add(new IncidentListPage(PageMode.NewOnly));
            this.Children.Add(new IncidentListPage(PageMode.Interval));
        }
    }
}