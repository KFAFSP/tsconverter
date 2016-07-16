namespace TibSunLegacy.FileFormats.Vxl
{
    public sealed class SpanPrototype
    {
        public readonly byte Start;

        public SpanPrototype(byte AStart)
        {
            this.Start = AStart;
        }

        public byte Count { get; set; }
    }
}