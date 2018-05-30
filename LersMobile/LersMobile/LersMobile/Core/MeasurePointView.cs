using System;
using System.Collections.Generic;
using System.Text;
using Lers.Core;
using Xamarin.Forms;

namespace LersMobile.Core
{
    /// <summary>
    /// Параметры точки учёта, используемые для вывода на экран.
    /// </summary>
    public class MeasurePointView
    {
        public MeasurePoint MeasurePoint { get; private set; }

        public string Title => this.MeasurePoint.Title;

        /// <summary>
        /// Источник изображения с состоянием точки учёта.
        /// </summary>
        public string StateImageSource
        {
            get
            {
                switch (this.MeasurePoint.State)
                {
                    case MeasurePointState.Normal: return "State_Normal.png";
                    case MeasurePointState.Error: return "State_Error.png";
                    case MeasurePointState.None: return "State_Unknown.png";
                    case MeasurePointState.Warning: return "State_Warning.png";
                    default:
                        throw new NotSupportedException("Неизвестное состояние " + this.MeasurePoint.State);
                }
            }
        }

        /// <summary>
        /// Изображение типа системы.
        /// </summary>
        public string SystemTypeImageSource
        {
            get
            {
                switch (this.MeasurePoint.SystemType)
                {
                    case SystemType.ColdWater:
                        return "SystemType_ColdWater.png";

                    case SystemType.Electricity:
                        return "SystemType_Electricity.png";

                    case SystemType.Gas:
                        return "SystemType_Gas.png";

                    case SystemType.Heat:
                        return "SystemType_Heat.png";

                    case SystemType.HotWater:
                        return "SystemType_HotWater.png";

                    case SystemType.Sewage:
                        return "SystemType_Sewage.png";

                    case SystemType.Steam:
                        return "SystemType_Steam.png";

                    default:
                        throw new NotSupportedException("Неизвестный тип системы: " + this.MeasurePoint.SystemType);
                }
            }
        }
        public MeasurePointView(MeasurePoint measurePoint)
        {
            this.MeasurePoint = measurePoint ?? throw new ArgumentNullException(nameof(measurePoint));
        }

    }
}
