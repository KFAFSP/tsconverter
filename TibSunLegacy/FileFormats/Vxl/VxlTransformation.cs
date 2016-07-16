using System;
using System.IO;

using FMath.Linear.Numeric;

using TibSunLegacy.Data;
using TibSunLegacy.Util;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlTransformation :
        IPersistable,
        IAssignable<VxlTransformation>
    {
        private readonly AffineFltMatrix FMatrix;

        public VxlTransformation()
        {
            this.FMatrix = AffineFltMatrix.Identitiy;
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

            float fDet = AffineFltMatrix.Determinant(this.FMatrix);
            AStream.WriteFloat(fDet);
            for (int I = 0; I < 12; I++)
                AStream.WriteFloat(this.FMatrix[I/4, I%4]);
        }
        #endregion

        public AffineFltMatrix Matrix
        {
            get { return this.FMatrix; }
        }
    }
}