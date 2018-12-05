using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Platform.Main.Annotations;
using Hdc.Mv.Halcon;
using Block = Hdc.Mv.Halcon.Block;

namespace Platform.Main.Views
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for BlockModule.xaml
    /// </summary>
    public sealed partial class BlockModule : UserControl, INotifyPropertyChanged, IBlockModule
    {
        private Block _block;
        public BlockModule(Block block)
        {
            _block = block; 
            
            InitializeComponent();            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Bindable(true)]
        public Block Block
        {
            get => _block;

            set
            {
                _block = value;
                OnPropertyChanged(nameof(Block));
            }
        }

        public void Refresh()
        {
            Block = Block;
        }
    }
}
