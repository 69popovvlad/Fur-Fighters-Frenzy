namespace Core.Application.Events
{
    public interface IUnityUpdateListener
    {
        void OnUpdate(float delta);
    }
}