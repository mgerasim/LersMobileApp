using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
		/// Полное наименование точки учёта, включая наименование объекта.
		/// </summary>
		public string FullTitle => this.MeasurePoint.FullTitle;

		/// <summary>
		/// Связанное с точкой учёта оборудование.
		/// </summary>
		public Equipment Device => this.MeasurePoint.Device;

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
        public string SystemTypeImageSource => ResourceHelper.GetSystemTypeImage(this.MeasurePoint.SystemType);


        public MeasurePointView(MeasurePoint measurePoint)
        {
            this.MeasurePoint = measurePoint ?? throw new ArgumentNullException(nameof(measurePoint));
        }

		/// <summary>
		/// Загружает требуемые для отображения данные по точке учёта.
		/// </summary>
		/// <returns></returns>
		public async Task LoadData()
		{
			await App.Core.EnsureConnected();

			var requiredFlags = MeasurePointInfoFlags.Equipment;

			if (!this.MeasurePoint.AvailableInfo.HasFlag(requiredFlags))
			{
				await this.MeasurePoint.RefreshAsync(requiredFlags);
			}
		}
    }
}
