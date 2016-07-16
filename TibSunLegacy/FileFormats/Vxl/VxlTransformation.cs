using System;
using System.IO;

using TibSunLegacy.Data;
using TibSunLegacy.Math;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlTransformation :
        IPersistable,
        IAssignable<VxlTransformation>
    {
        private readonly MatAffFlt FMatrix;

        public VxlTransformation()
        {
            this.FMatrix = MatAffFlt.Identity;
        }

        public void Assign(VxlTransformation AOther)
        {
            this.FMatrix.Assign(AOther.FMatrix);
        }

        #region IPersistable
        public void LoadFromStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            AStream.Skip(4);
            for (int I = 0; I < 12; I++)
                this.FMatrix[I/4, I%4] = AStream.ReadFloat();
        }
        public void SaveToStream(Stream AStream)
        {
            if (AStream == null)
                throw new ArgumentNullException("AStream");

            float fDet = MatAffFlt.Determinant(this.FMatrix);
            AStream.WriteFloat(fDet);
            for (int I = 0; I < 12; I++)
                AStream.WriteFloat(this.FMatrix[I/4, I%4]);
        }
        #endregion

        public MatAffFlt Matrix
        {
            get { return this.FMatrix; }
        }
    }
}