using System;
using System.Collections.Generic;

namespace TibSunLegacy.Util
{
    public static class EnumerableExtensions
    {
        public const int C_HashShift = 0x4CB2F;

        public static IEnumerable<TType> Yield<TType>(this TType AType)
        {
            yield return AType;
        }

        public static void ForEach<TType>(
            this IEnumerable<TType> AEnumerable,
            Action<TType> AAction)
        {
            foreach (TType tElement in AEnumerable)
                AAction(tElement);
        }

        public static int GetShiftedHashCode<TType>(
            this TType[] AArray,
            IEqualityComparer<TType> AComparer)
        {
            if (AArray == null)
                throw new ArgumentNullException("AArray");
            if (AComparer == null)
                AComparer = EqualityComparer<TType>.Default;

            int iHash = AArray.Length;
            foreach (TType tValue in AArray)
                iHash = unchecked(iHash * EnumerableExtensions.C_HashShift + AComparer.GetHashCode(tValue));

            return iHash;
        }
    }
}
