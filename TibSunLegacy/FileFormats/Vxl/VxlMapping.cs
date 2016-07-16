using System;
using System.Linq;

using TibSunLegacy.Math;

namespace TibSunLegacy.FileFormats.Vxl
{
    public abstract class VxlMapping :
        IVxlMapping
    {
        public const uint C_MaxDenseMemSize = 5 * 1024 * 1024;
        public const uint C_MaxDenseVolume = VxlMapping.C_MaxDenseMemSize/2;

        #region Static factory methods
        public static VxlMapping New(Vec3Int ASize)
        {
            if (ASize.Aggregate(1, (AAccu, AComponent) => AAccu*AComponent) > VxlMapping.C_MaxDenseVolume)
                return new SparseVxlMapping(ASize.X, ASize.Y, ASize.Z);

            return new DenseVxlMapping(ASize.X, ASize.Y, ASize.Z);
        }
        #endregion

        private readonly Vec3Int FDimension;

        public event EventHandler<VxlMappingChangedEventArgs> Changed;

        protected VxlMapping(int AWidth, int AHeight, int ADepth)
        {
            if (AWidth < 0 || AWidth > 256)
                throw new ArgumentOutOfRangeException("AWidth");
            if (AHeight < 0 || AHeight > 256)
                throw new ArgumentOutOfRangeException("AHeight");
            if (ADepth < 0 || ADepth > 256)
                throw new ArgumentOutOfRangeException("ADepth");

            this.FDimension = new Vec3Int(AWidth, AHeight, ADepth);
        }

        protected virtual void OnAdded(Vec3Int ACoords)
        {
            EventHandler<VxlMappingChangedEventArgs> ehHandler = this.Changed;
            if (ehHandler != null)
                ehHandler(this, new VxlMappingChangedEventArgs(ACoords, VxlMappingChangedEventArgs.ChangeType.Added));
        }
        protected virtual void OnRemoved(Vec3Int ACoords)
        {
            EventHandler<VxlMappingChangedEventArgs> ehHandler = this.Changed;
            if (ehHandler != null)
                ehHandler(this, new VxlMappingChangedEventArgs(ACoords, VxlMappingChangedEventArgs.ChangeType.Removed));
        }
        protected virtual void OnModified(Vec3Int ACoords)
        {
            EventHandler<VxlMappingChangedEventArgs> ehHandler = this.Changed;
            if (ehHandler != null)
                ehHandler(this, new VxlMappingChangedEventArgs(ACoords, VxlMappingChangedEventArgs.ChangeType.Modified));
        }

        protected abstract VxlVoxel DoGet(Vec3Int ACoords);
        protected abstract bool DoSet(Vec3Int ACoords, VxlVoxel AVoxel);

        public VxlVoxel Get(Vec3Int ACoords)
        {
            if (ACoords[0] < 0 || ACoords[0] >= this.FDimension[0]
                || ACoords[1] < 0 || ACoords[1] >= this.FDimension[1]
                || ACoords[2] < 0 || ACoords[2] >= this.FDimension[2])
                throw new ArgumentOutOfRangeException("ACoords");

            return this.DoGet(ACoords);
        }
        public bool Set(Vec3Int ACoords, VxlVoxel AVoxel)
        {
            if (ACoords[0] < 0 || ACoords[0] >= this.FDimension[0]
                || ACoords[1] < 0 || ACoords[1] >= this.FDimension[1]
                || ACoords[2] < 0 || ACoords[2] >= this.FDimension[2])
                throw new ArgumentOutOfRangeException("ACoords");

            if (!this.DoSet(ACoords, AVoxel))
            {
                this.OnModified(ACoords);
                return false;
            }

            if (AVoxel.Set)
                this.OnAdded(ACoords);
            else
                this.OnRemoved(ACoords);

            return true;
        }

        public abstract void Clear();

        public Vec3Int Dimension
        {
            get { return this.FDimension; }
        }
        public abstract int VoxelCount { get; }
        public int Volume
        {
            get { return this.Dimension.X * this.FDimension.Y * this.FDimension.Z; }
        }
    }
}