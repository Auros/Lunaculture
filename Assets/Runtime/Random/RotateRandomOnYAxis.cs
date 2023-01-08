using UnityEngine;
using Random = UnityEngine.Random;

namespace Lunaculture
{
    public class RotateRandomOnYAxis : MonoBehaviour
    {
        private void Start()
        {
            var localRot = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(localRot.x, Random.Range(0, 360), localRot.z);
        }
    }
}