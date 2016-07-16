using System;
using System.IO;

using TibSunLegacy.Data;
using TibSunLegacy.Math;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlBounds :
        IPersistable,
        IAssignable<VxlBounds>
    {
        private readonly Vec3Flt FMinimum;
        private readonly Vec3Flt FMaximum;

        public VxlBounds()
        {
            this.FMinimum = new Vec3Flt();
            this.FMaximum = new Vec3Flt();
        }

        public void Assign(VxlBounds AFrom)
        {
            this.FMinimum.Assign(AFrom.FMinimum);
            this.FMaximum.Assign(AFrom.FMaximum);
        }

        #region IPersistable
        public void LoadFromStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            for (int I = 0; I < 3; I++)
                this.FMinimum[I] = AStream.ReadFloat();
            for (int I = 0; I < 3; I++)
                this.FMaximum[I] = AStream.ReadFloat();
        }
        public void SaveToStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            for (int I = 0; I < 3; I++)
                AStream.WriteFloat(this.FMinimum[I]);
            for (int I = 0; I < 3; I++)
                AStream.WriteFloat(this.FMaximum[I]);
        }
        #endregion

        public Vec3Flt Minimum
        {
            get { return this.FMinimum; }
        }
        public Vec3Flt Maximum
        {
            get { return this.FMaximum; }
        }
    }
}