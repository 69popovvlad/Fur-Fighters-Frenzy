using System;
using System.Collections.Generic;

namespace Core.Entities.Views
{
    public static class ViewsContainer
    {
        private static readonly Dictionary<string, EntityView> _views = new Dictionary<string, EntityView>();

        public static string GenerateEntityGuid()
        {
            var guid = string.Empty;
            while (string.IsNullOrEmpty(guid) || _views.ContainsKey(guid))
            {
                guid = Guid.NewGuid().ToString();
            }

            return guid;
        }

        public static EntityView GetView(string guid) =>
            _views[guid];
        
        public static T GetView<T>(string guid) where T: EntityView =>
            _views[guid] as T;

        internal static void AddEntity(EntityView view) =>
            _views.Add(view.Guid, view);
        
        internal static void RemoveEntity(EntityView view) =>
            _views.Remove(view.Guid);
    }
}