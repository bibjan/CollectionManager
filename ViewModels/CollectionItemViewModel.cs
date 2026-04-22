using CollectionManager.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.ViewModels
{
    public class CollectionItemViewModel : ObservableObject
    {
        private Collection _collection;

        public string Name => _collection.Name;

        public ObservableCollection<Item> Items => _collection.Items;

        public CollectionItemViewModel(Collection collection)
        {
            _collection = collection;
        }
    }
}
