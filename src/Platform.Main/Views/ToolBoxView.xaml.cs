using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Core;
using Core.Presentation;
using Hdc.Mv.Halcon;
using Microsoft.Practices.Prism;
using Microsoft.Practices.ServiceLocation;
using UserControl = System.Windows.Controls.UserControl;

namespace Platform.Main.Views
{
    /// <inheritdoc cref="IToolBoxView" />
    /// <summary>
    /// Interaction logic for ToolBoxView.xaml
    /// </summary>
    public partial class ToolBoxView : UserControl, IToolBoxView
    {
        private ToolBox _toolBox;
        public ObservableCollection<BlockEntry> BlockEntries { get; set; }
            = new ObservableCollection<BlockEntry>();

        public ToolBoxView()
        {
            InitializeComponent();

            GetAllBlockType();

            CreateToolWindow();

            WindowsFormsHost.Child = _toolBox;
        }


        private void GetAllBlockType()
        {
            var blocks = ServiceLocator.Current.GetAllInstances<IBlock>();

            foreach (var block in blocks)
            {
                var T = block.GetType();

                var attribute= T.GetAttribute<BlockAttribute>();

                var entry = new BlockEntry
                {
                    Name = T.Name.Replace("Block", ""),
                    Type = T,
                    Catagory = attribute.Catagory
                };

                BlockEntries.Add(entry);
            }
        }

        #region ToolBox Control

        private void CreateToolWindow()
        {
            _toolBox = new ToolBox
            {
                ItemNormalColor = Color.BlanchedAlmond,
                ItemSelectedColor = Color.BurlyWood,
                ItemHoverColor = Color.BurlyWood
            };

            foreach (var blockCatagory in Enum.GetValues(typeof(BlockCatagory)))
            {
                _toolBox.AddTab($"{blockCatagory}", -1);
            }

            foreach (var entry in BlockEntries)
            {
                _toolBox[$"{entry.Catagory}"].AddItem(entry.Name, 0, 0, true, entry);
            }

            _toolBox[0].Selected = true;

        }

        #endregion
    }

    [Serializable]
    public class BlockEntry
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public BlockCatagory Catagory { get; set; }
    }    
}
