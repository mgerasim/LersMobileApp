using Lers.Core;
using System;
using System.Collections.Generic;
using System.Text;

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
                    throw new NotSupportedException("Неизвестный тип системы: " + systemType);
            }
        }
    }
}
