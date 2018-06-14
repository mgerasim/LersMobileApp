﻿using Lers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.Controls
{
	/// <summary>
	/// Отображает запись с данными по точке учёта.
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MeasurePointDataControl : ContentView
	{
		public static readonly BindableProperty DisplayRecordProperty = BindableProperty.Create(
			nameof(DisplayRecord), 
			typeof(DataRecord), 
			typeof(DataRecord), 
			null, 
			BindingMode.OneWay);


		/// <summary>
		/// Отображаемая запись с данными.
		/// </summary>
		public DataRecord DisplayRecord
		{
			get => (DataRecord)GetValue(DisplayRecordProperty);
			set => SetValue(DisplayRecordProperty, value);
		}


		/// <summary>
		/// Создаёт экземпляр объекта.
		/// </summary>
		public MeasurePointDataControl()
		{
			InitializeComponent();
		}


		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == DisplayRecordProperty.PropertyName)
			{
				DisplayDataRecord(this.DisplayRecord);
			}
		}

		/// <summary>
		/// Отображает данные по точке учёта.
		/// </summary>
		/// <param name="dataRecord"></param>
		private void DisplayDataRecord(DataRecord dataRecord)
		{
			// Удаляем существующие записи.

			this.dataGrid.RowDefinitions.Clear();
			this.dataGrid.Children.Clear();

			if (dataRecord == null)
			{
				return;
			}

			int rowNumber = 0;

			// Добавляем последние данные в грид.

			foreach (var record in dataRecord)
			{
				var desc = DataParameterDescriptor.Get(record.Key);

				var valueColor = record.Value.IsBad ? Color.LightCoral : Color.Default;

				string unit = desc.SystemUnitTitle;

				if (desc.IsAdditive)
				{
					unit += "/ч.";
				}

				var parameterLabel = new Label { Text = $"{desc.ShortTitle}" };
				var valueLabel = new Label { Text = $"{record.Value.Value:0.00}", BackgroundColor = valueColor };
				var unitLabel = new Label { Text = $"{unit}" };

				var rowDef = new RowDefinition();

				this.dataGrid.RowDefinitions.Add(rowDef);

				this.dataGrid.Children.Add(parameterLabel, 0, rowNumber);
				this.dataGrid.Children.Add(valueLabel, 1, rowNumber);
				this.dataGrid.Children.Add(unitLabel, 2, rowNumber);

				++rowNumber;
			}
		}
	}
}