using Core.Entities.Views;
using UnityEngine;

namespace Core.Entities.Examples
{
    public class ViewBuilderExample: ViewBuilderBase
    {
        protected override void Initialize()
        {
            ViewsFactory.AddBuilder<EntityViewExample>(CreateEntityViewExample);
        }

        private static void CreateEntityViewExample(object data, out GameObject instance, out Component component)
        {
            var prefab = new GameObject("Prefab mock");
            prefab.AddComponent<EntityViewExample>();
            
            instance = Object.Instantiate(prefab);
            
            var viewComponent = instance.GetComponent<EntityViewExample>();
            viewComponent.Initialize(data);
            component = viewComponent;
        }
    }
}