using System.Collections.Generic;
using UnityEngine;

namespace Core.Ui
{
    [CreateAssetMenu(fileName = "Windows Lookup", menuName = "Client/Configs/Windows Lookup", order = 0)]
    public class WindowsLookupConfig: ScriptableObject
    {
        [SerializeField] private List<WindowBase> _lookup = new List<WindowBase>();

        public IReadOnlyList<WindowBase> Lookup => _lookup;

        internal void UpdateLookup(List<WindowBase> windows)
        {
            _lookup = windows;
        }
    }
}