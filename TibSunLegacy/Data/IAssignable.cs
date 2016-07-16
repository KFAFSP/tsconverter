namespace TibSunLegacy.Data
{
    public interface IAssignable<in TInput>
    {
        void Assign(TInput AFrom);
    }
}