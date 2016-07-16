using TibSunLegacy.Math;

namespace TibSunLegacy.FileFormats.Vxl
{
    public interface IVxlMapping
    {
        VxlVoxel Get(Vec3Int ACoords);
        bool Set(Vec3Int ACoords, VxlVoxel AVoxel);

        void Clear();

        Vec3Int Dimension { get; }
        int VoxelCount { get; }
    }
}