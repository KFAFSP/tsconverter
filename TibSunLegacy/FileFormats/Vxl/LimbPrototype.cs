namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class LimbPrototype
    {
        public readonly VxlLimbHead Head;
        public readonly VxlLimbTail Tail;

        public LimbPrototype()
        {
            this.Head = new VxlLimbHead();
            this.Tail = new VxlLimbTail();
        }
        public LimbPrototype(VxlLimb ALimb)
        {
            this.Instance = ALimb;

            this.Head = new VxlLimbHead
            {
                Name = ALimb.Name,
                Number = ALimb.Number
            };
            this.Tail = new VxlLimbTail
            {
                NormalType = ALimb.NormalType,
            };

            this.Tail.Bounds.Assign(ALimb.Bounds);
            this.Tail.Size.Assign(ALimb.Mapping.Dimension);
            this.Tail.Transformation.Assign(ALimb.Transformation);
        }

        public VxlLimb Instance { get; set; }
    }
}