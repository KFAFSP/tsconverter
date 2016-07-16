using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using TibSunLegacy.Math;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlReader :
        IDisposable
    {
        private readonly Stream FStream;

        private readonly VxlFileHeader FFileHeader;
        private readonly VxlModel FModel;
        private readonly List<LimbPrototype> FLimbPrototypes;

        public VxlReader(Stream AStream, VxlModel AModel)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");
            if (AModel == null)
                throw new ArgumentNullException("AModel");

            if (!AStream.CanSeek)
                throw new NotSupportedException("Stream does not support seeking.");

            this.FStream = AStream;
            
            this.FFileHeader = new VxlFileHeader();
            this.FModel = AModel;
            this.FLimbPrototypes = new List<LimbPrototype>();
        }
        ~VxlReader()
        {
            this.Dispose(false);
        }

        private void Dispose(bool ADisposing)
        {
            if (this.PropagateDispose)
                this.FStream.Dispose();

            GC.SuppressFinalize(this);
        }
        public void Dispose()
        {
            this.Dispose(true);
        }

        private void ReadHeaders()
        {
            this.FLimbPrototypes.Clear();
            this.FStream.Seek(0, SeekOrigin.Begin);

            // File Header
            this.FFileHeader.LoadFromStream(this.FStream);

            // Header validation
            if (!this.FFileHeader.FileMagic.Equals(VxlFileHeader.C_FileMagic, StringComparison.Ordinal))
                throw new FormatException("Input stream is not a .vxl file.");
            if (this.FFileHeader.PaletteCount != 1)
                throw new FormatException("Only one palette supported.");

            // Palette
            this.FModel.Palette.LoadFromStream(this.FStream);

            // Calculate offsets
            this.LimbBodiesOffset = VxlLimbHead.C_LimbHeadsOffset
                                    + this.FFileHeader.LimbCount*VxlLimbHead.C_Size;
            this.LimbTailsOffset = this.LimbBodiesOffset + this.FFileHeader.BodySectionSize;

            // Limb prototypes
            for (int I = 0; I < this.FFileHeader.LimbCount; I++)
            {
                LimbPrototype lpProto = new LimbPrototype();
                this.FLimbPrototypes.Add(lpProto);

                // Head
                this.FStream.Seek(VxlLimbHead.C_LimbHeadsOffset + I*VxlLimbHead.C_Size, SeekOrigin.Begin);
                lpProto.Head.LoadFromStream(this.FStream);

                // Tail
                this.FStream.Seek(this.LimbTailsOffset + I*VxlLimbTail.C_Size, SeekOrigin.Begin);
                lpProto.Tail.LoadFromStream(this.FStream);
            }
        }
        private void ReadAllLimbs(VxlLimbFactory AFactory)
        {
            for (int I = 0; I < this.FLimbPrototypes.Count; I++)
                this.ReadLimb(I, AFactory);
        }
        private VxlLimb ReadLimb(int APrototypeIndex, VxlLimbFactory AFactory)
        {
            if (AFactory == null)
                throw new ArgumentNullException("AFactory");

            // Create limb
            LimbPrototype lpProto = this.FLimbPrototypes[APrototypeIndex];
            VxlLimb vlLimb = AFactory(lpProto.Head.Number, lpProto.Tail.Size);
            vlLimb.Name = lpProto.Head.Name;
            vlLimb.NormalType = lpProto.Tail.NormalType;
            vlLimb.Bounds.Assign(lpProto.Tail.Bounds);
            vlLimb.Transformation.Assign(lpProto.Tail.Transformation);
            lpProto.Instance = vlLimb;

            int iSpanCount = lpProto.Tail.Size.X * lpProto.Tail.Size.Z;
            this.FStream.Seek(this.LimbBodiesOffset + lpProto.Tail.SpanStartPointersOffset, SeekOrigin.Begin);

            // Read pointer lists
            VxlLimbBodyHeader vlbhHeader = new VxlLimbBodyHeader(iSpanCount);
            vlbhHeader.LoadFromStream(this.FStream);

            // Calculate absolute data offset
            long iLimbSpanDataOffset = this.LimbBodiesOffset + lpProto.Tail.SpanDataOffset;

            // Read spans
            for (int iSpan = 0; iSpan < iSpanCount; iSpan++)
            {
                if (vlbhHeader.StartPointers[iSpan] == -1)
                    continue;
                
                byte X = (byte)(iSpan%lpProto.Tail.Size.X);
                byte Z = (byte)(iSpan/lpProto.Tail.Size.X);
                byte Y = 0;

                this.FStream.Seek(iLimbSpanDataOffset + vlbhHeader.StartPointers[iSpan], SeekOrigin.Begin);

                // Read section until span is filled
                do
                {
                    byte bSkip = this.FStream.SafeReadByte();
                    Y += bSkip;
                    byte bCount = this.FStream.SafeReadByte();
                    for (byte bOffset = 0; bOffset < bCount; bOffset++)
                    {
                        vlLimb.Mapping.Set(
                            new Vec3Int(X, Y, Z),
                            new VxlVoxel(this.FStream.SafeReadByte(), this.FStream.SafeReadByte()));
                        Y++;
                    }
                    this.FStream.Skip(1);

                    Debug.WriteLine("READ: Span {0} {1} skip {2} count {3}", X, Z, bSkip, bCount);
                } while (Y < lpProto.Tail.Size.Y);
            }

            return vlLimb;
        }

        public void ReadToEnd(VxlLimbFactory AFactory = null)
        {
            if (AFactory == null)
                AFactory = (ANumber, ASize) => this.FModel.AddLimb(ANumber, ASize);

            this.ReadHeaders();
            this.ReadAllLimbs(AFactory);
        }

        public Stream BaseStream { get { return this.FStream; } }
        public bool PropagateDispose { get; set; }

        public VxlModel Model { get { return this.FModel; } }

        public long LimbBodiesOffset { get; private set; }
        public long LimbTailsOffset { get; private set; }
    }
}
