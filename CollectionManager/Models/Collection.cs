using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CollectionManager.Models
{
    public class Collection
    {
        public string Name { get; set; } = "";
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

        public Collection(string name)
        {
            Name = name;
        }
    }
}
