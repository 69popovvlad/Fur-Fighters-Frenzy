namespace Core.Utilities
{
    public interface ICloneable<out T> where T: ICloneable<T>
    {
        T Clone(bool deep = false);
    }
}
