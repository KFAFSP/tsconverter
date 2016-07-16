using System;
using System.IO;

using TibSunLegacy.Data;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlFileHeader :
        IPersistable
    {
        public const int C_Size = 32;

        public const string C_FileMagic = "Voxel Animation";

        private string FFileMagic;

        public VxlFileHeader()
        {
            this.FFileMagic = VxlFileHeader.C_FileMagic;
            this.PaletteCount = 1;
        }

        #region IPersistable
        public void LoadFromStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            this.FFileMagic = AStream.ReadAscii(16);
            this.PaletteCount = AStream.ReadUInt32();
            this.LimbCount = AStream.ReadUInt32();
            AStream.Skip(4);
            this.BodySectionSize = AStream.ReadUInt32();
        }
        public void SaveToStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            AStream.WriteAscii(this.FileMagic, 16);
            AStream.WriteUInt32(this.PaletteCount);
            AStream.WriteUInt32(this.LimbCount);
            AStream.WriteUInt32(this.LimbCount);
            AStream.WriteUInt32(this.BodySectionSize);
        }
        #endregion

        public string FileMagic
        {
            get { return this.FFileMagic; }
        }
        public uint PaletteCount { get; set; }
        public uint LimbCount { get; set; }
        public uint BodySectionSize { get; set; }
    }
}