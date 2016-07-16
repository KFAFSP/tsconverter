using System.IO;

namespace TibSunLegacy.Data
{
    public interface IPersistable
    {
        void LoadFromStream(Stream AStream);
        void SaveToStream(Stream AStream);
    }
}
