using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppImage.Common
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isLoad;
        public bool IsLoad
        {
            get { return isLoad; }
            set
            {
                if (value != isLoad)
                {
                    isLoad = value;
                    OnPropertyChanged(nameof(IsLoad));
                }
            }
        }
    }
}
