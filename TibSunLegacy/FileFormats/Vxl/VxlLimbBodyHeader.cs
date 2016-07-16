using System;
using System.IO;

using TibSunLegacy.Data;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlLimbBodyHeader :
        IPersistable
    {
        private readonly int FSpanCount;
        private readonly int[] FStartPointers;
        private readonly int[] FEndPointers;

        public VxlLimbBodyHeader(int ASpanCount)
        {
            this.FSpanCount = ASpanCount;
            this.FStartPointers = new int[ASpanCount];
            this.FEndPointers = new int[ASpanCount];
        }

        #region IPersistable
        public void LoadFromStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            for (int I = 0; I < this.FSpanCount; I++)
                this.FStartPointers[I] = AStream.ReadInt32();

            for (int I = 0; I < this.FSpanCount; I++)
                this.FEndPointers[I] = AStream.ReadInt32();
        }
        public void SaveToStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            for (int I = 0; I < this.FSpanCount; I++)
                AStream.WriteInt32(this.FStartPointers[I]);

            for (int I = 0; I < this.FSpanCount; I++)
                AStream.WriteInt32(this.FEndPointers[I]);
        }
        #endregion

        public int SpanCount
        {
            get { return this.FSpanCount; }
        }
        public int[] StartPointers
        {
            get { return this.FStartPointers; }
        }
        public int[] EndPointers
        {
            get { return this.FEndPointers; }
        }
    }
}