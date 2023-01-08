using Lunaculture.GameTime;
using UnityEngine;

namespace Lunaculture
{
    public class DayNightCycleController : MonoBehaviour
    {
        [SerializeField] private TimeController timeController = null!;
        [SerializeField] private Vector3 baseRotation = Vector3.zero;
        [SerializeField] private Vector3 rotationDirection = Vector3.right;
        [SerializeField] private Vector3 sinRotationDirection = Vector3.up;
        [SerializeField] private float sinRotationStrength = 30f;

        private void Update()
        {
            transform.eulerAngles = baseRotation
                + (timeController.DayProgress * 360 * rotationDirection)
                + (Mathf.Sin(timeController.DayProgress * Mathf.PI * 2) * sinRotationStrength * sinRotationDirection);
        }
    }
}
