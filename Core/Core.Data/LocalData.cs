using System;
using System.IO;

namespace Core.Data
{
    public abstract class LocalData
    {
        protected string Name { get; private set; }

        public abstract void OnCreate();
        
        public abstract void OnLoad();

        public virtual void SaveInternal()
        {
            if (!Directory.Exists(DataService.LocalPath))
            {
                throw new Exception($"{nameof(DataService.LocalPath)} should be initialized before");
            }

            DataService.Save(this, $"{DataService.LocalPath}/{Name}{DataService.FileFormat}");
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new Exception($"{nameof(Name)} field should be initialized before in {GetType()}");
            }

            SaveInternal();
        }

        internal void SetFilename(string name)
        {
            Name = name;
        }
    }
}