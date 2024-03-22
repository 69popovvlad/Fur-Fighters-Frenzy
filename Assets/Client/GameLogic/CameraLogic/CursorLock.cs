using UnityEngine;

namespace Client.GameLogic.CameraLogic
{
    public class CursorLock : MonoBehaviour
    {
        [SerializeField] private bool _lockCursor = true;

        private void Start()
        {
            if (!_lockCursor)
            {
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.U))
            {
                return;
            }

            _lockCursor = !_lockCursor;
            Cursor.lockState = _lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !_lockCursor;
        }
    }
}