using System;
using System.IO;

using TibSunLegacy.Data;
using TibSunLegacy.Math.Numeric;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlLimbTail :
        IPersistable
    {
        public const int C_Size = 92;

        private readonly VxlTransformation FTransformation;
        private readonly VxlBounds FBounds;
        private readonly Vec3Int FSize;

        public VxlLimbTail()
        {
            this.FTransformation = new VxlTransformation();
            this.FBounds = new VxlBounds();
            this.FSize = new Vec3Int();
        }

        #region IPersistable
        public void LoadFromStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            this.SpanStartPointersOffset = AStream.ReadUInt32();
            this.SpanEndPointersOffset = AStream.ReadUInt32();
            this.SpanDataOffset = AStream.ReadUInt32();
            this.FTransformation.LoadFromStream(AStream);
            this.FBounds.LoadFromStream(AStream);
            this.FSize[0] = AStream.SafeReadByte();
            this.FSize[2] = AStream.SafeReadByte();
            this.FSize[1] = AStream.SafeReadByte();
            this.NormalType = (VxlNormalType)AStream.SafeReadByte();
        }
        public void SaveToStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            AStream.WriteUInt32(this.SpanStartPointersOffset);
            AStream.WriteUInt32(this.SpanEndPointersOffset);
            AStream.WriteUInt32(this.SpanDataOffset);
            this.FTransformation.SaveToStream(AStream);
            this.FBounds.SaveToStream(AStream);
            AStream.WriteByte((byte)this.FSize[0]);
            AStream.WriteByte((byte)this.FSize[2]);
            AStream.WriteByte((byte)this.FSize[1]);
            AStream.WriteByte((byte)this.NormalType);
        }
        #endregion

        public uint SpanStartPointersOffset { get; set; }
        public uint SpanEndPointersOffset { get; set; }
        public uint SpanDataOffset { get; set; }
        public VxlTransformation Transformation
        {
            get { return this.FTransformation; }
        }
        public VxlBounds Bounds
        {
            get { return this.FBounds; }
        }
        public Vec3Int Size
        { 
            get { return this.FSize; } 
        }
        public VxlNormalType NormalType { get; set; }
    }
}