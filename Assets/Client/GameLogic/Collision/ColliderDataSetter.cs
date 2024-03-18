using UnityEngine;

namespace Client.GameLogic.Collision
{
    public class ColliderDataSetter: MonoBehaviour
    {
        [SerializeField] private ColliderDataControl[] _colliders;

        private static int _id;
        
        private static int NextId => ++_id;

        private void Awake()
        {
            var id = ColliderDataSetter.NextId;
            for (int i = 0, iLen = _colliders.Length; i < iLen; ++i)
            {
                _colliders[i].SetId(id);
            }
        }
    }
}