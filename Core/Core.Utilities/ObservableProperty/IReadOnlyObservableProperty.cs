namespace Core.Utilities.ObservableProperty
{
    public interface IReadOnlyObservableProperty<out T>: IPropertyChangedEvent
    {
        T Value { get; }
    }
}