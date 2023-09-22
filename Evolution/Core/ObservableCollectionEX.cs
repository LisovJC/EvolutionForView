using Evolution.Services.DataSaveLoadServices;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Evolution.Core
{
    public class ObservableCollectionEX<T> : ObservableCollection<T>
    {
        public ObservableCollectionEX() : base()
        {
            CollectionChanged += ObservableCollection_CollectionChanged;
        }
        private void ObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (T item in e.OldItems)
                {
                    if (item != null && item is INotifyPropertyChanged i)
                    {
                        i.PropertyChanged -= Element_PropertyChanged;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (T item in e.NewItems)
                {
                    if (item != null && item is INotifyPropertyChanged i)
                    {
                        i.PropertyChanged -= Element_PropertyChanged;
                        i.PropertyChanged += Element_PropertyChanged;
                    }
                }
            }
            DataSaveLoad.Serialize(this);
        }

        private void Element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DataSaveLoad.Serialize(this);
        }
    }
}