using Core.Application;

namespace Core.Entities
{
    public abstract class EntityBuilderBase: IApplicationResource
    {
        protected EntityBuilderBase()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Initialize();
        }

        protected abstract void Initialize();
    }
}