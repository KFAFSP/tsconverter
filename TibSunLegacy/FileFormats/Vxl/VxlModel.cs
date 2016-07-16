using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TibSunLegacy.Math;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlModel :
        IEnumerable<VxlLimb>
    {
        public static void ValidateName(string AName)
        {
            if (AName == null)
                throw new ArgumentNullException("AName");

            if (AName.Length > 16)
                throw new ArgumentOutOfRangeException("AName");
            if (AName.Contains("\x00") || AName.Any(AChar => Convert.ToUInt32(AChar) > 0xEF))
                throw new ArgumentException("Name contains invalid characters.");
        }

        private readonly VxlPalette FPalette;
        private readonly Dictionary<uint, VxlLimb> FLimbs;

        public VxlModel()
        {
            this.FPalette = new VxlPalette();
            this.FLimbs = new Dictionary<uint, VxlLimb>();
        }

        public VxlLimb AddLimb(uint ANumber, Vec3Int ASize)
        {
            VxlLimb vlLimb = new VxlLimb(ANumber, ASize);
            this.AddLimb(vlLimb);
            return vlLimb;
        }
        public VxlLimb AddLimb(uint ANumber, VxlMapping AMapping)
        {
            VxlLimb vlLimb = new VxlLimb(ANumber, AMapping);
            this.AddLimb(vlLimb);
            return vlLimb;
        }
        public void AddLimb(VxlLimb ALimb)
        {
            if (this.FLimbs.ContainsKey(ALimb.Number))
                throw new ArgumentException(String.Format("Limb with index {0} already exists.", ALimb.Number));

            this.FLimbs[ALimb.Number] = ALimb;
        }
        
        public bool RemoveLimb(uint ANumber)
        {
            return this.FLimbs.Remove(ANumber);
        }
        public bool RemoveLimb(VxlLimb ALimb)
        {
            VxlLimb vlCurrent;
            if (!this.FLimbs.TryGetValue(ALimb.Number, out vlCurrent))
                return false;

            if (!Object.ReferenceEquals(vlCurrent, ALimb))
                return false;

            this.FLimbs.Remove(ALimb.Number);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public IEnumerator<VxlLimb> GetEnumerator()
        {
            return this.FLimbs.Values.GetEnumerator();
        }

        public VxlPalette Palette
        {
            get { return this.FPalette; }
        }

        public int LimbCount
        {
            get { return this.FLimbs.Count; }
        }
        public IEnumerable<VxlLimb> Limbs
        {
            get { return this.FLimbs.Values; }
        }

        public VxlLimb this[uint ALimb]
        {
            get { return this.FLimbs[ALimb]; }
        }
    }
}
