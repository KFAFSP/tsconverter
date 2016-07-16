using System.Drawing;

namespace TibSunLegacy.FileFormats.Vxl
{
    public struct VxlPaletteRgb
    {
        #region Static factory methods
        public static VxlPaletteRgb FromArgb(int ARGB)
        {
            return new VxlPaletteRgb(
                (byte)(ARGB & 0xFF),
                (byte)((ARGB >> 8) & 0xFF),
                (byte)((ARGB >> 16) & 0xFF));
        }
        public static VxlPaletteRgb FromColor(Color AColor)
        {
            return new VxlPaletteRgb(AColor.R, AColor.G, AColor.B);
        }
        #endregion

        public readonly byte R, G, B;

        public VxlPaletteRgb(byte AR, byte AG, byte AB)
        {
            this.R = AR;
            this.G = AG;
            this.B = AB;
        }

        public int Argb
        {
            get { return this.B | this.G << 8 | this.R << 16; }
        }
        public Color Color
        {
            get { return Color.FromArgb(0xFF, this.R, this.G, this.B); }
        }
    }
}
