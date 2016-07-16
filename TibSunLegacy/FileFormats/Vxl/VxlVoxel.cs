namespace TibSunLegacy.FileFormats.Vxl
{
    public struct VxlVoxel
    {
        public static readonly VxlVoxel Empty = new VxlVoxel();

        public readonly bool Set;
        public readonly byte PaletteIndex;
        public readonly byte NormalIndex;

        public VxlVoxel(
            byte APaletteIndex,
            byte ANormalIndex)
        {
            this.Set = true;
            this.PaletteIndex = APaletteIndex;
            this.NormalIndex = ANormalIndex;
        }
    }
}
