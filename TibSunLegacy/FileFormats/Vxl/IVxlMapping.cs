using FMath.Linear.Numeric;

namespace TibSunLegacy.FileFormats.Vxl
{
    public interface IVxlMapping
    {
        VxlVoxel Get(Vector3Int ACoords);
        bool Set(Vector3Int ACoords, VxlVoxel AVoxel);

        void Clear();

        Vector3Int Dimension { get; }
        int VoxelCount { get; }
    }
}