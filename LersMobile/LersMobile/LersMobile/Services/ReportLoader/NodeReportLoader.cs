﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lers.Reports;
using LersMobile.Views;

namespace LersMobile.Core.ReportLoader
{
	/// <summary>
	/// Реализация загрузки отчетов для объекта учета
	/// </summary>
    public class NodeReportLoader : IReportLoader
    {
        NodesReportLoader ReportLoaderNodes;
        
        public NodeReportLoader(NodeView nodeView)
        {
            var nodes = new List<NodeView>();
            nodes.Add(nodeView ?? throw new ArgumentNullException());

            ReportLoaderNodes = new NodesReportLoader(nodes);
        }

        public int[] GetEntitiesIds()
        {
            return ReportLoaderNodes.GetEntitiesIds();
        }

        public ReportEntity GetReportEntity()
        {
            return ReportLoaderNodes.GetReportEntity();
        }

        public List<ReportsView> GetReports()
        {
            return ReportLoaderNodes.GetReports();
        }

        public async Task Reload(bool isForce = false)
        {
            await ReportLoaderNodes.Reload(isForce);
        }
    }
}
