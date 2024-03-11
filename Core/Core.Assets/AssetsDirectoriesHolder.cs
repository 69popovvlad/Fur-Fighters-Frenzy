using System;
using UnityEngine;

namespace Core.Assets
{
    [CreateAssetMenu(fileName = "AssetsDirectoriesHolder", menuName = "Client/Assets/Directories holder", order = 0)]
    public class AssetsDirectoriesHolder : ScriptableObject
    {
        [SerializeField] private AssetsDirectoriesData[] allDirectories = Array.Empty<AssetsDirectoriesData>();

        public AssetsDirectoriesData[] AllDirectories => allDirectories;
    }
}