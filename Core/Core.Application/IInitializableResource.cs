namespace Core.Application
{
    /// <summary>
    /// Used for classes that will be instantiated when application is started and after that initialized
    /// </summary>
    public interface IInitializableResource: IApplicationResource
    {
        void Initialize();
    }
}