using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Core.Collections.Generic;
using Hdc.Mv.Halcon;
using Platform.Main.Annotations;
using Platform.Main.Util;
using Block = Hdc.Mv.Halcon.Block;

namespace Platform.Main.Views
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for ChangePortReferenceWindow.xaml
    /// </summary>
    public sealed partial class ChangePortReferenceWindow : Window, INotifyPropertyChanged
    {
        private PortReference _editingPortReference;

        public ChangePortReferenceWindow()
        {
            InitializeComponent();
        }

        public ObservableCollection<string> InputBlockNames { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> RefBlockNames { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> RefPortNames { get; set; } = new ObservableCollection<string>();

        public PortReference EditingPortReference
        {
            get => _editingPortReference;
            set
            {
                if (Equals(value, _editingPortReference)) return;
                _editingPortReference = value;
                OnPropertyChanged(nameof(EditingPortReference));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (EditingPortReference.SourceBlockName == null)
            {
                MessageBox.Show("SourceBlockName is empty!");
                return;
            }

            if (EditingPortReference.SourcePortName == null)
            {
                MessageBox.Show("SourcePortName is empty!");
                return;
            }

            DialogResult = true;
            this.Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private BlockSchema _blockSchema;
        private Block _block;
        private string _selectTargetPortName;
        private List<string> _targetPortName;


        public List<string> TargetPortName
        {
            get => _targetPortName;
            set
            {
                if (Equals(value, _targetPortName)) return;
                _targetPortName = value;
                OnPropertyChanged(nameof(TargetPortName));
            }
        }

        public string SelectTargetPortName
        {
            get => _selectTargetPortName;
            set
            {
                if (Equals(value, _selectTargetPortName)) return;
                _selectTargetPortName = value;
                OnPropertyChanged(nameof(SelectTargetPortName));
            }
        }

        public bool? ShowDialog(BlockSchema blockSchema, Block block, List<string> targetPortName, PortReference portReference = null)
        {
            _blockSchema = blockSchema;
            _block = block;
            TargetPortName = targetPortName;
            SelectTargetPortName = targetPortName[0];

            EditingPortReference = portReference ?? new PortReference(SelectTargetPortName);

            //
            RefBlockNames.Clear();
            RefPortNames.Clear();

            var names = _blockSchema.Blocks.Select(x => x.Name).ToList();
            names.Remove(_block.Name);

            RefBlockNames.AddRange(names);

            //
            if (portReference != null)
            {
                var sourceBlock = _blockSchema.Blocks.SingleOrDefault(x => x.Name == portReference.SourceBlockName);
                var portNames = sourceBlock.GetPropertyNamesByAttributeType<OutputPortAttribute>();
                RefPortNames.AddRange(portNames);
            }

            return ShowDialog();
        }

        private void RefBlockNamesComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefPortNames.Clear();

            var sourceBlock = _blockSchema.Blocks.SingleOrDefault(x => x.Name == EditingPortReference.SourceBlockName);
            var portNames = sourceBlock.GetPropertyNamesByAttributeType<OutputPortAttribute>();

            RefPortNames.AddRange(portNames);
        }
    }
}