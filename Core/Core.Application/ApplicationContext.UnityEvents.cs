using UnityEngine;

namespace Core.Application
{
    public partial class ApplicationContext
    {
        private void Update()
        {
            var delta = Time.deltaTime;

            for (var i = 0; i < _updateListeners.Count; ++i)
            {
                _updateListeners[i].OnUpdate(delta);
            }
        }

        private void OnApplicationPause(bool pause)
        {
            for (var i = 0; i < _applicationPauseListeners.Count; ++i)
            {
                _applicationPauseListeners[i].OnApplicationPause(pause);
            }
        }

        private void OnApplicationQuit()
        {
            for (var i = 0; i < _applicationQuitListeners.Count; ++i)
            {
                _applicationQuitListeners[i].OnApplicationQuit();
            }
        }
    }
}
