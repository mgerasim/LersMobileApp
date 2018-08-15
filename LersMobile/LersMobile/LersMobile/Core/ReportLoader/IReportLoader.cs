using LersMobile.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LersMobile.Core.ReportLoader
{
    public interface IReportLoader
    {
        Task Reload(bool isForce = false);
        List<ReportViewCollectionGrouping> GetReports();
    }
}
