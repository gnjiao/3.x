using HalconDotNet;

namespace Hdc.Mv.Halcon
{
    public static class HWindowExtensions
    {
        public static void SetDraw(this HWindow window, RegionFillMode fillMode)
        {
            var halconString = fillMode.ToHalconString();
            window.SetDraw(halconString);
        }

        public static void SetDrawFill(this HWindow window)
        {
            window.SetDraw("fill");
        }

        public static void SetDrawMargin(this HWindow window)
        {
            window.SetDraw("margin");
        }
    }
}