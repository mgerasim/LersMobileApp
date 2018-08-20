using System.ComponentModel;

namespace LersMobile.Core
{
	/// <summary>
	/// Класс-контейнер множественного выбора в списке данных
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class SelectableData<T> : INotifyPropertyChanged
    {
		/// <summary>
		/// Элемент данных
		/// </summary>
        public T Data { get; set; }

		/// <summary>
		/// Признак того, что элемент данных выбран
		/// </summary>
        private bool selected = false;

        public bool IsSelected
        {
            get => IsSelecting && selected;
            set
            {
                selected = value;
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(IsUnselected));
            }
        }

		/// <summary>
		/// Признак того, что список элементов данных в режиме множественного выбора
		/// </summary>
        private bool isSelecting;        

        public bool IsSelecting
        {
            get => isSelecting;
            set
            {
                isSelecting = value;
                OnPropertyChanged(nameof(IsSelecting));
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(IsUnselected));
            }
        }
		/// <summary>
		/// Признак того, что элемент данных не выбран
		/// </summary>
        public bool IsUnselected => !IsSelected && IsSelecting;

		#region INotifyPropertyChanged implement interface

		public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


    }
}
