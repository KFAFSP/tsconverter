using System.Collections.Generic;

using FMath.Linear.Numeric;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class SparseVxlMapping :
        VxlMapping
    {
        private readonly Dictionary<Vector3Int, VxlVoxel> FMapping;

        public SparseVxlMapping(int AWidth, int AHeight, int ADepth)
            : base(AWidth, AHeight, ADepth)
        {
            this.FMapping = new Dictionary<Vector3Int, VxlVoxel>(AWidth * AHeight * ADepth);
        }

        protected override VxlVoxel DoGet(Vector3Int ACoords)
        {
            VxlVoxel vvGet;
            if (!this.FMapping.TryGetValue(ACoords, out vvGet))
                return VxlVoxel.Empty;

            return vvGet;
        }
        protected override bool DoSet(Vector3Int ACoords, VxlVoxel AVoxel)
        {
            if (!AVoxel.Set)
                return this.FMapping.Remove(ACoords);

            bool bNew = !this.FMapping.ContainsKey(ACoords);
            this.FMapping[ACoords] = AVoxel;
            return bNew;
        }

        public override void Clear()
        {
            this.FMapping.Clear();
        }

        public override int VoxelCount
        {
            get { return this.FMapping.Count; }
        }
    }
}