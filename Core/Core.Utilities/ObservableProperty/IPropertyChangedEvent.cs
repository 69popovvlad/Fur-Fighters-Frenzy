namespace Core.Utilities.ObservableProperty
{
    public delegate void PropertyChanged(string name, object newValue, object lastValue);
    
    public interface IPropertyChangedEvent
    {
        event PropertyChanged PropertyChanged;
    }
}
