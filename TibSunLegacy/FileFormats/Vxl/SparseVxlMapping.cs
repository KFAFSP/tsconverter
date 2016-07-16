using System.Collections.Generic;

using TibSunLegacy.Math;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class SparseVxlMapping :
        VxlMapping
    {
        private readonly Dictionary<Vec3Int, VxlVoxel> FMapping;

        public SparseVxlMapping(int AWidth, int AHeight, int ADepth)
            : base(AWidth, AHeight, ADepth)
        {
            this.FMapping = new Dictionary<Vec3Int, VxlVoxel>(AWidth * AHeight * ADepth);
        }

        protected override VxlVoxel DoGet(Vec3Int ACoords)
        {
            VxlVoxel vvGet;
            if (!this.FMapping.TryGetValue(ACoords, out vvGet))
                return VxlVoxel.Empty;

            return vvGet;
        }
        protected override bool DoSet(Vec3Int ACoords, VxlVoxel AVoxel)
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