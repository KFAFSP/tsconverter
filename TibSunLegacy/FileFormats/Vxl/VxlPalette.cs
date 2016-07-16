using System;
using System.IO;

using TibSunLegacy.Data;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlPalette :
        IPersistable
    {
        public const int C_Size = 770;

        public const byte C_TibSunRemapStart = 0x10;
        public const byte C_TibSunRemapEnd = 0x1F;

        private readonly VxlPaletteRgb[] FColors;

        public VxlPalette()
        {
            this.FColors = new VxlPaletteRgb[256];

            this.RemapStartIndex = VxlPalette.C_TibSunRemapStart;
            this.RemapEndIndex = VxlPalette.C_TibSunRemapEnd;
        }

        #region IPersistable
        public void LoadFromStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            this.RemapStartIndex = AStream.SafeReadByte();
            this.RemapEndIndex = AStream.SafeReadByte();

            byte bIndex = 0;
            do
            {
                this.FColors[bIndex] = new VxlPaletteRgb(
                    AStream.SafeReadByte(),
                    AStream.SafeReadByte(),
                    AStream.SafeReadByte());
                unchecked { bIndex++; }
            } while (bIndex != 0);
        }
        public void SaveToStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            AStream.WriteByte(this.RemapStartIndex);
            AStream.WriteByte(this.RemapEndIndex);

            byte bIndex = 0;
            do
            {
                AStream.WriteByte(this.FColors[bIndex].R);
                AStream.WriteByte(this.FColors[bIndex].G);
                AStream.WriteByte(this.FColors[bIndex].B);
                unchecked { bIndex++; }
            } while (bIndex != 0);
        }
        #endregion

        public byte RemapStartIndex { get; set; }
        public byte RemapEndIndex { get; set; }
    }
}