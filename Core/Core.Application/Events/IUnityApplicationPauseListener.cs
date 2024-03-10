namespace Core.Application.Events
{
    public interface IUnityApplicationPauseListener
    {
        void OnApplicationPause(bool pause);
    }
}