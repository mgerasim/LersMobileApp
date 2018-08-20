using Lers;
using Lers.Core;
using LersMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Services.Resource
{
    public static class ResourceService
    {
		/// <summary>
		/// Возвращает имя изображения для типа системы точки учёта.
		/// </summary>
		/// <param name="systemType"></param>
		/// <returns></returns>
		public static string SystemTypeImage(SystemType systemType)
		{
			switch (systemType)
			{
				case SystemType.ColdWater: return "SystemType_ColdWater.png";
				case SystemType.Electricity: return "SystemType_Electricity.png";
				case SystemType.Gas: return "SystemType_Gas.png";
				case SystemType.Heat: return "SystemType_Heat.png";
				case SystemType.HotWater: return "SystemType_HotWater.png";
				case SystemType.Sewage: return "SystemType_Sewage.png";
				case SystemType.Steam: return "SystemType_Steam.png";

				default:
					throw new NotSupportedException($"{Droid.Resources.Messages.ResourceHelper_Not_Supported_SystemType}:  + {systemType}");
			}
		}

		public static string IncidentImportanceImage(Lers.Diag.IncidentImportance importance)
		{
			switch (importance)
			{
				case Lers.Diag.IncidentImportance.Critical: return "Importance_Error32.png";
				case Lers.Diag.IncidentImportance.Information: return "Importance_Info32.png";
				case Lers.Diag.IncidentImportance.Warning: return "Importance_Warn32.png";

				default: throw new NotSupportedException($"{Droid.Resources.Messages.ResourceHelper_Not_Supported_Incident_Importance}: " + importance);
			}
		}

		public static string ImportanceImage(Importance importance)
		{
			switch (importance)
			{
				case Importance.FatalError: return "Importance_Critical32.png";
				case Importance.Info: return "Importance_Info32.png";
				case Importance.Warn: return "Importance_Warn32.png";
				case Importance.Debug: return "Importance_Debug32.png";
				case Importance.Error: return "Importance_Error32.png";


				default: throw new NotSupportedException($"{Droid.Resources.Messages.ResourceHelper_Not_Supported_Importance}: " + importance);
			}
		}


		public static string NodeStateImage(NodeState nodeState)
		{
			switch (nodeState)
			{
				case NodeState.None: return "State_Unknown.png";
				case NodeState.Error: return "State_Error.png";
				case NodeState.Normal: return "State_Normal.png";
				case NodeState.Warning: return "State_Warning.png";

				default:
					throw new NotSupportedException($"{Droid.Resources.Messages.Text_State_Not_Supported}: " + nodeState);
			}
		}


		public static string MeasurePointStateImage(MeasurePointState state)
		{
			switch (state)
			{
				case MeasurePointState.None: return "State_Unknown.png";
				case MeasurePointState.Error: return "State_Error.png";
				case MeasurePointState.Normal: return "State_Normal.png";
				case MeasurePointState.Warning: return "State_Warning.png";

				default:
					throw new NotSupportedException($"{Droid.Resources.Messages.Text_State_Not_Supported} " + state);
			}
		}

		public static string ConnectionTypeImage(Lers.Poll.CommunicationLink link)
		{
			switch (link)
			{
				case Lers.Poll.CommunicationLink.Dialup: return "CommLinkType_Dialup.png";
				case Lers.Poll.CommunicationLink.Direct: return "CommLinkType_Direct.png";
				case Lers.Poll.CommunicationLink.Gprs: return "CommLinkType_Gprs.png";
				case Lers.Poll.CommunicationLink.Gsm: return "CommLinkType_Gsm.png";
				case Lers.Poll.CommunicationLink.Ip: return "CommLinkType_Ip.png";

				default: throw new ArgumentOutOfRangeException($"{Droid.Resources.Messages.ResourceHelper_Not_Supported_Communication_Link}: {link}");
			}
		}

		/// <summary>
		/// Источник изображения с состоянием объекта учёта.
		/// </summary>
		public static string NodeImageSource(Node node)
		{			
			switch (node.State)
			{
				case NodeState.Error: return "node_red.png";
				case NodeState.Normal: return "node_green.png";
				case NodeState.Warning: return "node_orange.png";
				case NodeState.None: return "node_gray.png";
				default:
					throw new NotSupportedException(node.State.ToString());
			}			
		}

		/// <summary>
		/// Источник изображения с состоянием точки учёта.
		/// </summary>
		public static string StateImageSource(MeasurePoint measurePoint)
		{			
			switch (measurePoint.State)
			{
				case MeasurePointState.Normal: return "State_Normal.png";
				case MeasurePointState.Error: return "State_Error.png";
				case MeasurePointState.None: return "State_Unknown.png";
				case MeasurePointState.Warning: return "State_Warning.png";
				default:
					throw new NotSupportedException($"{Droid.Resources.Messages.Text_State_Not_Supported} {measurePoint.State}");
			}
		}		

	}
}
