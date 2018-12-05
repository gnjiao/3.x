using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [ContentProperty("Blocks")]
    public class BlockSchema
    {
        //public Collection<Block> Blocks { get; set; } = new BindingList<Block>();        

        public ObservableCollection<Block> Blocks { get; set; } = new ObservableCollection<Block>();
    }
}