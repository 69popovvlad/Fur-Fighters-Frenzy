namespace Core.Entities.Examples
{
    public static class BuilderExample
    {
        static BuilderExample()
        {
            EntitiesFactory.AddBuilder<EntityExample>(CreateEntityExample);
        }

        // Create entity instance and view for it if necessary
        private static IEntity CreateEntityExample(object data) =>
            new EntityExample();

        private class EntityExample: EntityBase {}
    }
}