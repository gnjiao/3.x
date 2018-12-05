using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public abstract class DefinitionBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string GroupName { get; set; }
        public double PixelCellSideLengthInMillimeter { get; set; }
        public UnitType UnitType { get; set; } = UnitType.Pixel;

        public bool SaveCacheImageEnabled { get; set; }
        public string SaveCacheImageFileName { get; set; }

        public string CoordinateName { get; set; } = "Default";
        public string ColorName { get; set; }

        /// <summary>
        /// the height per gray value in milimeter
        /// </summary>
        public double ZScaleInMillimeter { get; set; }
    }
}