using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public static class EntitiesContainer
    {
        private static readonly Dictionary<string, IEntity> _entities = new Dictionary<string, IEntity>();

        public static string GenerateEntityGuid()
        {
            var guid = string.Empty;
            while (string.IsNullOrEmpty(guid) || _entities.ContainsKey(guid))
            {
                guid = Guid.NewGuid().ToString();
            }

            return guid;
        }

        public static IEntity GetEntity(string guid) =>
            _entities[guid];

        internal static void AddEntity(IEntity entity) =>
            _entities.Add(entity.Guid, entity);
        
        internal static void RemoveEntity(IEntity entity) =>
            _entities.Remove(entity.Guid);
    }
}