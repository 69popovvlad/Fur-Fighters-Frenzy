using Core.Ioc;
using UnityEngine;

namespace Core.Ui
{
    public class WindowBase: MonoBehaviour
    {
        // ReSharper disable once UnassignedField.Global
        [Inject] protected WindowsSystem WindowsSystem;

        public virtual void Initialize(object data)
        {
            /* Nothing to do */
        }
    }
}