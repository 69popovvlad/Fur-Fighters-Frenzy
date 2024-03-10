using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public static class EntitiesFactory
    {
        public delegate IEntity CreateEntityDelegate(object data);

        private static readonly Dictionary<Type, CreateEntityDelegate> _builders =
            new Dictionary<Type, CreateEntityDelegate>();

        public static void AddBuilder<T>(CreateEntityDelegate @delegate) where T : IEntity =>
            _builders.Add(typeof(T), @delegate);

        public static IEntity CreateEntity<T>(object data) where T : IEntity =>
            _builders[typeof(T)].Invoke(data);
    }
}