using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using TibSunLegacy.Math.Numeric;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlWriter :
        IDisposable
    {
        private readonly Stream FStream;

        private readonly VxlFileHeader FFileHeader;
        private readonly VxlModel FModel;
        private readonly List<LimbPrototype> FLimbPrototypes;

        public VxlWriter(Stream AStream, VxlModel AModel)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");
            if (!AStream.CanSeek)
                throw new NotSupportedException("Stream does not support seeking.");
            if (AModel == null)
                throw new ArgumentNullException("AModel");

            this.FStream = AStream;

            this.FFileHeader = new VxlFileHeader();
            this.FModel = AModel;
            this.FLimbPrototypes = new List<LimbPrototype>();
        }
        ~VxlWriter()
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

        private void WriteAllLimbs()
        {
            this.FStream.Seek(this.LimbBodiesOffset, SeekOrigin.Begin);

            // Write all limbs
            for (int I = 0; I < this.FLimbPrototypes.Count; I++)
                this.WriteLimb(I);

            // Get tails offset
            this.LimbTailsOffset = this.FStream.Position;

            // Set body length
            this.FFileHeader.BodySectionSize = (uint)(this.LimbTailsOffset - this.LimbBodiesOffset);
        }
        private IEnumerable<SpanPrototype> EnumSections(VxlMapping AMapping, byte AX, byte AZ)
        {
            SpanPrototype spCurrent = null;
            byte AY = 0;

            int iVoxels = 0;
            Vec3Int vPos = new Vec3Int(AX, 0, AZ);
            while (vPos.Y < AMapping.Dimension.Y)
            {
                if (AMapping.Get(vPos).Set)
                {
                    if (spCurrent == null)
                        spCurrent = new SpanPrototype((byte)vPos.Y) { Count = 1 };
                    else
                        spCurrent.Count++;

                    iVoxels++;
                }
                else if (spCurrent != null)
                {
                    yield return spCurrent;
                    spCurrent = null;
                }

                vPos.Y++;
            }

            if (iVoxels == 0)
                yield break;

            if (spCurrent != null)
                yield return spCurrent;
            else
                yield return new SpanPrototype((byte)(AMapping.Dimension.Y)) { Count = 0 };
        }
        private void WriteLimb(int APrototypeIndex)
        {
            // Get prototype
            LimbPrototype lpProto = this.FLimbPrototypes[APrototypeIndex];

            int iSpanCount = lpProto.Tail.Size.X*lpProto.Tail.Size.Z;
            VxlLimbBodyHeader vlbhHeader = new VxlLimbBodyHeader(iSpanCount);

            // Save start pointers offset
            lpProto.Tail.SpanStartPointersOffset = (uint)(this.FStream.Position - this.LimbBodiesOffset);

            this.FStream.Skip(iSpanCount * 4);

            // Save end pointers offset
            lpProto.Tail.SpanEndPointersOffset = (uint)(this.FStream.Position - this.LimbBodiesOffset);

            this.FStream.Skip(iSpanCount * 4);

            // Save data offset
            long iDataOffset = this.FStream.Position;
            lpProto.Tail.SpanDataOffset = (uint)(this.FStream.Position - this.LimbBodiesOffset);

            // Write all spans and save their offsets
            for (int iSpan = 0; iSpan < iSpanCount; iSpan++)
            {
                byte X = (byte)(iSpan%lpProto.Tail.Size.X);
                byte Z = (byte)(iSpan/lpProto.Tail.Size.X);

                long iStart = this.FStream.Position;

                SpanPrototype spLast = null;
                foreach (SpanPrototype spProto in this.EnumSections(lpProto.Instance.Mapping, X, Z))
                {
                    byte bSkip;
                    if (spLast == null)
                        bSkip = spProto.Start;
                    else
                        bSkip = (byte)(spProto.Start - spLast.Start - spLast.Count);

                    this.FStream.WriteByte(bSkip);
                    this.FStream.WriteByte(spProto.Count);
                    Vec3Int vPos = new Vec3Int(X, spProto.Start, Z);
                    for (byte bOffset = 0; bOffset < spProto.Count; bOffset++)
                    {
                        VxlVoxel vvVoxel = lpProto.Instance.Mapping.Get(vPos);
                        this.FStream.WriteByte(vvVoxel.PaletteIndex);
                        this.FStream.WriteByte(vvVoxel.NormalIndex);
                        vPos.Y++;
                    }
                    this.FStream.WriteByte(spProto.Count);
                    Debug.WriteLine("SAVE: Span {0} {1} skip {2} count {3}", X, Z, bSkip, spProto.Count);

                    spLast = spProto;
                }

                if (spLast == null)
                {
                    vlbhHeader.StartPointers[iSpan] = -1;
                    vlbhHeader.EndPointers[iSpan] = -1;
                }
                else
                {
                    vlbhHeader.StartPointers[iSpan] = (int)(iStart - iDataOffset);
                    vlbhHeader.EndPointers[iSpan] = (int)(this.FStream.Position - iDataOffset - 1);
                }
            }

            // Go back and write the pointers
            long iEnd = this.FStream.Position;
            this.FStream.Seek(lpProto.Tail.SpanStartPointersOffset + this.LimbBodiesOffset, SeekOrigin.Begin);
            vlbhHeader.SaveToStream(this.FStream);

            // Return to end of limb
            this.FStream.Seek(iEnd, SeekOrigin.Begin);
        }
        private void WriteHeaders()
        {
            this.FStream.Seek(0, SeekOrigin.Begin);

            // File header
            this.FFileHeader.SaveToStream(this.FStream);

            // Palette
            this.FModel.Palette.SaveToStream(this.FStream);

            // Limb prototypes
            for (int I = 0; I < this.FFileHeader.LimbCount; I++)
            {
                LimbPrototype lpProto = this.FLimbPrototypes[I];

                // Head
                this.FStream.Seek(VxlLimbHead.C_LimbHeadsOffset + I * VxlLimbHead.C_Size, SeekOrigin.Begin);
                lpProto.Head.SaveToStream(this.FStream);

                // Tail
                this.FStream.Seek(this.LimbTailsOffset + I * VxlLimbTail.C_Size, SeekOrigin.Begin);
                lpProto.Tail.SaveToStream(this.FStream);
            }
        }

        public void WriteToEnd()
        {
            // Create prototypes
            this.FLimbPrototypes.Clear();
            this.FModel.Limbs.ForEach(ALimb => this.FLimbPrototypes.Add(new LimbPrototype(ALimb)));

            // Set limb count
            this.FFileHeader.LimbCount = (uint)this.FLimbPrototypes.Count;

            // Calculate bodies offset
            this.LimbBodiesOffset = VxlLimbHead.C_LimbHeadsOffset
                                    + this.FFileHeader.LimbCount * VxlLimbHead.C_Size;

            this.WriteAllLimbs();
            this.WriteHeaders();
        }

        public Stream BaseStream { get { return this.FStream; } }
        public bool PropagateDispose { get; set; }

        public VxlModel Model { get { return this.FModel; } }

        public long LimbBodiesOffset { get; private set; }
        public long LimbTailsOffset { get; private set; }
    }
}