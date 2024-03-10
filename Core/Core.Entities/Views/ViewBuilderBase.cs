using Core.Application;

namespace Core.Entities.Views
{
    public abstract class ViewBuilderBase: IApplicationResource
    {
        protected ViewBuilderBase()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Initialize();
        }

        protected abstract void Initialize();
    }
}