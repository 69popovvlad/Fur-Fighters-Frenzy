using Core.Entities.Views;

namespace Core.Entities.Examples
{
    // Instance of this builder you can get from Ioc
    public class EntityBuilderExample : EntityBuilderBase
    {
        protected override void Initialize()
        {
            EntitiesFactory.AddBuilder<EntityExample>(CreateEntityExample);
        }

        // Create entity instance and view for it if necessary
        private static IEntity CreateEntityExample(object data)
        {
            var entity = new EntityExample(data);
            // Example to using with generic type
            // var entity = (TEntity)Activator.CreateInstance(typeof(TEntity), new object[] { cardData });

            // ReSharper disable once InlineTemporaryVariable
            var loadedData = data; // loading data here if necessary
            ViewsFactory.CreateView<EntityViewExample>(loadedData, out var gmObject, out var component);

            return entity;
        }
    }
}