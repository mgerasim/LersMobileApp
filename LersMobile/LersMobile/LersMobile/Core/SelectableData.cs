using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LersMobile.Core
{
    public class SelectableData<T> : INotifyPropertyChanged
    {
        public T Data { get; set; }

        private bool selected = false;

        public bool IsSelected
        {
            get
            {
                return IsSelecting && selected;
            }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(IsUnselected));
            }
        }

        private bool isSelecting;        

        public bool IsSelecting
        {
            get
            {
                return isSelecting;
            }
            set
            {
                isSelecting = value;
                OnPropertyChanged(nameof(IsSelecting));
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(IsUnselected));
            }
        }

        public bool IsUnselected
        {
            get
            {
                return !IsSelected && IsSelecting;
            }
        }

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
