using System;

namespace TibSunLegacy.Data
{
    public interface ICloneable<out TResult> :
        ICloneable
    {
        new TResult Clone();
    }
}