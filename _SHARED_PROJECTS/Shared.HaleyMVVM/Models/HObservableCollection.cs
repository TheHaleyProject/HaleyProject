using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace Haley.Models
{
    //Whenever a collection is changed, like something is added or removed, notifications are raised. However, when the properties inside the objects of the collection are changed, we never get notified. So to solve this issue, we need to find a way to trigger the collectionchanged event whenever a property of the object changes.
    //Base idea is that we register a handler for the propertychanged events of the objects. Whenever this property changes, we trigger the collectionchanged event.
    //We can also setup some delegate methods to perform boolean operations to validate whether to trigger the collectionchanged event or not.

    public sealed class HObservableCollection<T>  : ObservableCollection<T>
        where T: INotifyPropertyChanged //HObservableCollection will accept only items that has INotifyPropertyChanged implemented.
      //We are inheriting ObservableCollection Class.
      //We are adorning with a sealed modifier so that it will not be inherited by some other class.
    {
        private void _registerCollection()
        {
            //Since we have inherited observablecollection, we can merely make use of its properties.
            CollectionChanged += _collectionChanged;
        }

        //We have multiple ways how a list can be added to the Observable collection. It can be adding IEnumerable, or List or even Add items one by one.
        public delegate bool CanTriggerDelegate(object parameters); 

        public object trigger_params { get; set; }
        /// <summary>
        /// Set a delegate method which takes an object argument and returns a bool. Do remember to set your parameters as well. By default,the parameters are null.
        /// </summary>
        public CanTriggerDelegate trigger_delegate { get; set; }


        #region Collection Adding Methods

        /// <summary>
        /// Can add 
        /// </summary>
        /// <param name="Items"></param>
        public HObservableCollection(List<T> Items) : this() //You can also use Base(Items) in case you need to make use of the methods present inside the abstract classes. For new initialization
        {
            if (Items == null) return;
            //if (Items.Count == 0 || Items == null) return;
            foreach (var item in Items)
            {
                this.Add(item);
            }
            Items.ForEach(p => p.PropertyChanged += _propertyChanged);
        }

        public HObservableCollection(IEnumerable<T> Items) : this() //For new initialization
        {
            if (Items == null) return;
            //if (Items.ToList().Count == 0 || Items == null) return;
            foreach (var item in Items)
            {
                this.Add(item);
            }
            Items.ToList().ForEach(p => p.PropertyChanged += _propertyChanged);
        }

        public void addRange(List<T> Items) //This is add range, which will add the items to the existing values.
        {
            if (Items == null) return;
            //if (Items.Count == 0 || Items == null) return;
            foreach (var item in Items)
            {
                this.Add(item);
            }
            Items.ForEach(p => p.PropertyChanged += _propertyChanged);
        }

        public void addNew(T Item)
        {
            this.Add(Item);
            Item.PropertyChanged += _propertyChanged;
        }

        #endregion

        public HObservableCollection() : base()
        { _registerCollection(); }

        #region Event Handlers

        private void _collectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //By default when a collection changes, the notifypropertychanged will be invoked. So that whichever class implements it will be able to notify. However, earlier we are also registering all the properties of the objects inside the collection to invoke the trigger. We need to turn that off now. Else, even if the items are removed from the collection, we will still get the notifications
            if (e.NewItems != null)
            {
                foreach(var item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += _propertyChanged;
                }
            }
            if (e.OldItems != null) //Meaning that items have been removed. We need to remove the hanlder for their property changed events.
            {
                foreach(var item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= _propertyChanged;
                }
            }
        }

        private void _propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (trigger_delegate == null || trigger_delegate.Invoke(trigger_params) == true) //If the trigger is null or else the trigger returns true, then invoke collection changed.
            {
                NotifyCollectionChangedEventArgs TempArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
                OnCollectionChanged(TempArgs); //Invoking the collection changed event (of the base class), which in turn will be handled by the R_CollectionChanged
            }
        }

        #endregion

    }
}
