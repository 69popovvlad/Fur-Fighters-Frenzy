namespace Core.Reflection
{
    public interface IKeyableAttribute<T>
    {
        T Key { get; set; }
    }
}
