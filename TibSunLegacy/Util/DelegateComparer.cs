using System.Collections.Generic;

namespace TibSunLegacy.Util
{
    public sealed class DelegateComparer<TType> : IEqualityComparer<TType>, IComparer<TType>
    {
        #region IEqualityComparer<TType>
        public bool Equals(TType ALeft, TType ARight)
        {
            if (this.Equator == null)
                return EqualityComparer<TType>.Default.Equals(ALeft, ARight);

            return this.Equator(ALeft, ARight);
        }
        public int GetHashCode(TType AObject)
        {
            if (this.Hasher == null)
                return EqualityComparer<TType>.Default.GetHashCode(AObject);

            return this.Hasher(AObject);
        }
        #endregion

        #region IComparer<TType>
        public int Compare(TType ALeft, TType ARight)
        {
            if (this.Comparer == null)
                return Comparer<TType>.Default.Compare(ALeft, ARight);

            return this.Comparer(ALeft, ARight);
        }
        #endregion

        public EquatorDelegate<TType> Equator { get; set; }
        public HashingDelegate<TType> Hasher { get; set; }
        public ComparerDelegate<TType> Comparer { get; set; }
    }
}