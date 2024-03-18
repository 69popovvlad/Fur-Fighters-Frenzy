using Core.Application;

namespace Core.Entities
{
    public abstract class EntityBuilderBase: IInitializableResource
    {
        public abstract void Initialize();
    }
}