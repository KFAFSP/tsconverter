using FMath.Linear.Numeric;

namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class VxlLimb
    {
        private readonly uint FNumber;
        private string FName;
        private readonly VxlMapping FMapping;
        private readonly VxlTransformation FTransformation;
        private readonly VxlBounds FBounds;

        private VxlLimb(uint ANumber)
        {
            this.FNumber = ANumber;
            this.FName = "";
            this.FTransformation = new VxlTransformation();
            this.FBounds = new VxlBounds();
            this.NormalType = VxlNormalType.Ambiguous;
        }
        public VxlLimb(uint ANumber, Vector3Int ASize)
            : this(ANumber)
        {
            this.FMapping = VxlMapping.New(ASize);
        }
        public VxlLimb(uint ANumber, VxlMapping AMapping)
            : this(ANumber)
        {
            this.FMapping = AMapping;
        }

        public uint Number
        {
            get { return this.FNumber; }
        }
        public string Name
        {
            get { return this.FName; }
            set
            {
                VxlModel.ValidateName(value);

                this.FName = value;
            }
        }
        public VxlMapping Mapping
        {
            get { return this.FMapping; }
        }
        public VxlTransformation Transformation
        {
            get { return this.FTransformation; }
        }
        public VxlBounds Bounds
        {
            get { return this.FBounds; }
        }
        public VxlNormalType NormalType { get; set; }
    }
}
