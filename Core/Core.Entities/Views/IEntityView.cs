namespace Core.Entities.Views
{
    public interface IEntityView
    {
        string Guid { get; }
        
        void Initialize(IEntity entity);
    }
}