using Lers.Data;
using Lers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LersMobile.MeasurePointProperties
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MeasurePointArchivePage : ContentPage
	{
		private static DeviceDataType[] DataTypes = new DeviceDataType[]
		{
			DeviceDataType.Day, DeviceDataType.Hour, DeviceDataType.Month
		};

		private static String[] dateStringFormat = new String[]
		{
			"{0:dd.MM.yyyy}", "{0:dd.MM.yyyy HH:mm}", "{0:MM.yyyy}"
		};

		/// <summary>
		/// Выбранный для отображения тип данных.
		/// </summary>
		private DeviceDataType SelectedDataType => DataTypes[this.dataTypePicker.SelectedIndex];

		private readonly Core.MeasurePointView _measurePoint;

		private readonly Core.MobileCore core;

		private bool isLoaded = false;

		private String SelectedStringFormat => dateStringFormat[this.dataTypePicker.SelectedIndex];


		public ObservableCollection<DataRecord> Data { get; private set; } = new ObservableCollection<DataRecord>();

		private DataRecord _currentDataRecord;

		public DataRecord CurrentDataRecord
		{
			get => _currentDataRecord;
			set
			{
				_currentDataRecord = value;
				OnPropertyChanged(nameof(CurrentDataRecord));
			}
		}		
		
		public MeasurePointArchivePage(Core.MeasurePointView measurePoint)
		{
			BindingContext = this;

			InitializeComponent();

			this.core = App.Core;

			_measurePoint = measurePoint;

			SetColumns();

			FillDataTypes();

			this.BindingContext = this;

			this.Title = Droid.Resources.Messages.MeasurePointArchivePage_Title;

			BuilderTableHeader();


		}

		private void UpdateDataGrid()
		{




			/*
			if (containerStackLayout.Children.Count > 1)
			{

				containerStackLayout.Children.RemoveAt(1);
			}

			var headTemplate = new DataTemplate(() =>
			{
				var grid = new Grid();
				var dateTimeLabel = new Label();

				dateTimeLabel.Text = "D";

				grid.Children.Add(dateTimeLabel);

				return new ViewCell { View = grid };
				
			});
			
			var dataTemplate = new DataTemplate(() =>
			{
				var grid = new Grid();
				var dateTimeLabel = new Label();


				
				//dateTimeLabel.FormattedText

				dateTimeLabel.SetBinding(Label.TextProperty, "DateTime", BindingMode.Default, null, this.SelectedStringFormat);

				grid.Children.Add(dateTimeLabel);

				return new ViewCell { View = grid };
			});

			ListView listView = new ListView { ItemsSource = Data, ItemTemplate = dataTemplate };

			listView.HeaderTemplate = headTemplate;

			

			this.containerStackLayout.Children.Add(listView);

			*/

			/*
				while (dataGrid.Columns.Count > 0)
				{
					dataGrid.Columns.RemoveAt(0);
				}

				while (dataGrid.ColumnDefinitions.Count > 0)
				{
					dataGrid.ColumnDefinitions.RemoveAt(0);
				}
				*/
				
			if (containerStackLayout.Children.Count > 1)
			{
				containerStackLayout.Children.RemoveAt(1);
			}
			
			//Xamarin.Forms.DataGrid.DataGrid dataGrid = new Xamarin.Forms.DataGrid.DataGrid();

			/*
			Xamarin.Forms.DataGrid.ColumnCollection cols = new Xamarin.Forms.DataGrid.ColumnCollection();

			Xamarin.Forms.DataGrid.DataGridColumn columnItem = new Xamarin.Forms.DataGrid.DataGridColumn();

			columnItem.Title = "Дата";
			columnItem.PropertyName = "DateTime";
			columnItem.StringFormat = SelectedStringFormat;
						
			cols.Add(columnItem);

			dataGrid.Columns = cols;

			
			dataGrid.ItemsSource = Data;
			*/

			dataGrid.Columns[0].StringFormat = SelectedStringFormat;



			this.containerStackLayout.Children.Add(dataGrid);
		}

		private void BuilderTableHeader()
		{
			//var head = _measurePoint.MeasurePoint.DataParameters;

			//foreach(var param in head)
			//{
			//	//var p = _measurePoint.MeasurePoint.Data.Da
			//	var hd = Lers.Data.DataParameterDescriptor.Get(param);
			//}
		}


		private void SetColumns()
		{
		}

		private void FillDataTypes()
		{
			foreach (var dataType in DataTypes)
			{
				this.dataTypePicker.Items.Add(dataType.GetDescription());
			}

			this.dataTypePicker.SelectedIndex = 0;
		}

		/// <summary>
		/// Вызывается при отображении на экране.
		/// </summary>
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (this.isLoaded)
			{
				return;
			}
			
			this.isLoaded = true;

			await LoadRecords();


			UpdateDataGrid();
		}

		private void Filter_ToolbarItem_Clicked()
		{
			this.dataTypePicker.Focus();
		}

		private async Task LoadRecords()
		{
			this.IsBusy = true;

			try
			{
				DateTime endDate;

				if (this.CurrentDataRecord == null)
				{
					endDate = DateTime.Today.AddDays(1);
				}
				else
				{
					endDate = this.CurrentDataRecord.DateTime.AddSeconds(-1);
				}

				DateTime startDate = DateTimeUtils.Increment(endDate, this.SelectedDataType, -48);

				await this.core.EnsureConnected();

				var records = (await this._measurePoint.MeasurePoint.Data.GetConsumptionAsync(startDate, endDate, this.SelectedDataType)).OrderByDescending(x => x.DateTime);
				
				this.Data.Clear();
				this.Data.AddRange(records);

			}
			finally
			{
				this.IsBusy = false;
			}
		}

		public async void OnDataTypeSelected(object sender, EventArgs e)
		{
			if (!this.IsVisible)
			{
				return;
			}

			UpdateDataGrid();

			await LoadRecords();
		}

		private void dataGrid_Refreshing(object sender, EventArgs e)
		{
			UpdateDataGrid();
		}
	}
}