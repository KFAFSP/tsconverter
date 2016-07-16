using System;
using System.IO;

using TibSunLegacy.Data;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlLimbHead :
        IPersistable
    {
        public const int C_LimbHeadsOffset = VxlFileHeader.C_Size + VxlPalette.C_Size;
        public const int C_Size = 28;

        public const uint C_Unknown1 = 1;
        public const uint C_Unknown2 = 0;

        private string FName;

        public VxlLimbHead()
        {
            this.FName = "";
            this.Unknown1 = VxlLimbHead.C_Unknown1;
            this.Unknown2 = VxlLimbHead.C_Unknown2;
        }

        #region IPersistable
        public void LoadFromStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            this.FName = AStream.ReadAscii(16);
            this.Number = AStream.ReadUInt32();
            this.Unknown1 = AStream.ReadUInt32();
            this.Unknown2 = AStream.ReadUInt32();
        }
        public void SaveToStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            AStream.WriteAscii(this.FName, 16);
            AStream.WriteUInt32(this.Number);
            AStream.WriteUInt32(this.Unknown1);
            AStream.WriteUInt32(this.Unknown2);
        }
        #endregion

        public string Name
        {
            get { return this.FName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (value.Length > 16)
                    throw new ArgumentOutOfRangeException("value");

                this.FName = value;
            }
        }
        public uint Number { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
    }
}