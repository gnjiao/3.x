using System;

namespace Hdc.Mv.Halcon
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BlockAttribute : Attribute
    {
        public BlockAttribute()
        {
        }

        public BlockAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        public BlockAttribute(string displayName, string catagoryName)
        {
            DisplayName = displayName;
            CatagoryName = catagoryName;
        }

        public BlockAttribute(string displayName, BlockCatagory catagory)
        {
            DisplayName = displayName;
            Catagory = catagory;
        }

        public string DisplayName { get; set; }

        public string CatagoryName { get; set; }

        public BlockCatagory Catagory { get; set; }
    }
}