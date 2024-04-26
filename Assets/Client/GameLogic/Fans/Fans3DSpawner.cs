using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.GameLogic.Fans
{
    public class Fans3DSpawner : MonoBehaviour
    {
        [SerializeField] private Fan3DControl[] _fansPrefabs;
        [SerializeField] private Transform[] _fansPoints;
        [SerializeField] private Transform _lookAtPoint;

        private void Awake()
        {
            for(int i = 0, iLen = _fansPoints.Length; i < iLen; ++i)
            {
                var randomModel = _fansPrefabs[Random.Range(0, _fansPrefabs.Length)];
                var instance = Instantiate(randomModel, _fansPoints[i]);
                
                instance.transform.localPosition = Vector3.zero;
                instance.transform.LookAt(_lookAtPoint);
            }
        }
    }
}