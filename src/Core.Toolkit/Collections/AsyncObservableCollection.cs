﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace Core.Toolkit.Collections
{
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {        
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        public AsyncObservableCollection() { }
        public AsyncObservableCollection(IEnumerable<T> list) : base(list) { }
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {

            if (SynchronizationContext.Current == _synchronizationContext)
            {
                RaiseCollectionChanged(e);
            }
            else
            {    
                _synchronizationContext.Post(RaiseCollectionChanged, e);
            }
        }
        private void RaiseCollectionChanged(object param)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {         
                RaisePropertyChanged(e);
            }
            else
            {       
                _synchronizationContext.Post(RaisePropertyChanged, e);
            }
        }
        private void RaisePropertyChanged(object param)
        {  
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }
}
