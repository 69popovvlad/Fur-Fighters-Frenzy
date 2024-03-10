using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Entities.Views
{
    public static class ViewsFactory
    {
        public delegate void CreateViewDelegate(object data, out GameObject instance, out Component component);

        private static readonly Dictionary<Type, CreateViewDelegate> _builders =
            new Dictionary<Type, CreateViewDelegate>();

        public static void AddBuilder<T>(CreateViewDelegate @delegate) where T : Component =>
            _builders.Add(typeof(T), @delegate);

        public static void CreateView<T>(object data, out GameObject instance, out T component)
            where T : Component
        {
            _builders[typeof(T)].Invoke(data, out instance, out var rawComponent);
            component = (T)rawComponent;
        }
    }
}