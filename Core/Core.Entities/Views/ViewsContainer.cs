using System;
using System.Collections.Generic;

namespace Core.Entities.Views
{
    public static class ViewsContainer
    {
        private static readonly Dictionary<string, IEntityView> _views = new Dictionary<string, IEntityView>();

        public static string GenerateEntityGuid()
        {
            var guid = string.Empty;
            while (string.IsNullOrEmpty(guid) || _views.ContainsKey(guid))
            {
                guid = Guid.NewGuid().ToString();
            }

            return guid;
        }

        public static IEntityView GetView(string guid) =>
            _views[guid];
        
        public static T GetView<T>(string guid) where T: IEntityView =>
            (T)_views[guid];

        public static void AddEntity(IEntityView view) =>
            _views.Add(view.Guid, view);
        
        public static void RemoveEntity(IEntityView view) =>
            _views.Remove(view.Guid);
    }
}