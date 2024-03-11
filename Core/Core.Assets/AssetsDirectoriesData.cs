using System;

namespace Core.Assets
{
    [Serializable]
    public struct AssetsDirectoriesData
    {
        public string AssetsKey;
        public AssetsType AssetsType;
        public string[] Directories;
    }
}