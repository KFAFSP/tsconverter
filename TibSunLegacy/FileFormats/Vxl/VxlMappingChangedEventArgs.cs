using System;

using FMath.Linear.Numeric;

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

        private readonly Vector3Int FCoords;
        private readonly ChangeType FChange;

        public VxlMappingChangedEventArgs(Vector3Int ACoords, ChangeType AChange)
        {
            this.FCoords = ACoords;
            this.FChange = AChange;
        }

        public Vector3Int Coords
        {
            get { return this.FCoords; }
        }
        public ChangeType Change
        {
            get { return this.FChange; }
        }
    }
}