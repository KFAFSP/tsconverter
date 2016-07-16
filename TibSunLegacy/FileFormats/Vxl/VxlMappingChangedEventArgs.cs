using System;

using TibSunLegacy.Math;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlMappingChangedEventArgs : EventArgs
    {
        public enum ChangeType
        {
            Modified = 0,
            Added = 1,
            Removed = 2
        }

        private readonly Vec3Int FCoords;
        private readonly ChangeType FChange;

        public VxlMappingChangedEventArgs(Vec3Int ACoords, ChangeType AChange)
        {
            this.FCoords = ACoords;
            this.FChange = AChange;
        }

        public Vec3Int Coords
        {
            get { return this.FCoords; }
        }
        public ChangeType Change
        {
            get { return this.FChange; }
        }
    }
}