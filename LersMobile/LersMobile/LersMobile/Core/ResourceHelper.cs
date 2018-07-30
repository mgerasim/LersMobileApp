using Lers.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Lers;

namespace LersMobile.Core
{
    /// <summary>
    /// Содержит методы для получения ресурсов.
    /// </summary>
    static class ResourceHelper
    {
        /// <summary>
        /// Возвращает имя изображения для типа системы точки учёта.
        /// </summary>
        /// <param name="systemType"></param>
        /// <returns></returns>
        public static string GetSystemTypeImage(SystemType systemType)
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

        public static string GetIncidentImportanceImage(Lers.Diag.IncidentImportance importance)
        {
            switch (importance)
            {
                case Lers.Diag.IncidentImportance.Critical: return "Importance_Error32.png";
                case Lers.Diag.IncidentImportance.Information: return "Importance_Info32.png";
                case Lers.Diag.IncidentImportance.Warning: return "Importance_Warn32.png";

                default: throw new NotSupportedException($"{Droid.Resources.Messages.ResourceHelper_Not_Supported_Incident_Importance}: " + importance);
            }
        }

        public static string GetImportanceImage(Importance importance)
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


		public static string GetNodeStateImage(NodeState nodeState)
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


		public static string GetMeasurePointStateImage(MeasurePointState state)
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

		public static string GetConnectionTypeImage(Lers.Poll.CommunicationLink link)
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
	}
}
