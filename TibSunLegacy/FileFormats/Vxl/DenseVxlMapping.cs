using TibSunLegacy.Math;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class DenseVxlMapping :
        VxlMapping
    {
        private int FVoxelCount;
        private readonly VxlVoxel[,,] FMapping;

        public DenseVxlMapping(int AWidth, int AHeight, int ADepth)
            : base(AWidth, AHeight, ADepth)
        {
            this.FMapping = new VxlVoxel[AWidth,AHeight,ADepth];
            this.FVoxelCount = 0;
        }

        protected override void OnAdded(Vec3Int ACoords)
        {
            this.FVoxelCount++;
            base.OnAdded(ACoords);
        }
        protected override void OnRemoved(Vec3Int ACoords)
        {
            this.FVoxelCount--;
            base.OnRemoved(ACoords);
        }

        protected override VxlVoxel DoGet(Vec3Int ACoords)
        {
            return this.FMapping[ACoords.X, ACoords.Y, ACoords.Z];
        }
        protected override bool DoSet(Vec3Int ACoords, VxlVoxel AVoxel)
        {
            VxlVoxel vvOld = this.FMapping[ACoords.X, ACoords.Y, ACoords.Z];
            this.FMapping[ACoords.X, ACoords.Y, ACoords.Z] = AVoxel;
            return vvOld.Set != AVoxel.Set;
        }

        public override void Clear()
        {
            for (int X = 0; X < this.Dimension.X; X++)
                for (int Y = 0; Y < this.Dimension.Y; Y++)
                    for (int Z = 0; Z < this.Dimension.Z; Z++)
                        this.FMapping[X, Y, Z] = VxlVoxel.Empty;
        }

        public override int VoxelCount
        {
            get { return this.FVoxelCount; }
        }
    }
}