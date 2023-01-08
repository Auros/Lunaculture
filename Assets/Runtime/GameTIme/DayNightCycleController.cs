using Lunaculture.GameTime;
using UnityEngine;

namespace Lunaculture
{
    public class DayNightCycleController : MonoBehaviour
    {
        [SerializeField] private TimeController _timeController = null!;
        [SerializeField] private Light _light = null!;
        [SerializeField] private Vector3 _baseRotation = Vector3.zero;
        [SerializeField] private Vector3 _rotationDirection = Vector3.right;
        [SerializeField] private Vector3 _sinRotationDirection = Vector3.up;
        [SerializeField] private float _sinRotationStrength = 30f;

        private void Update()
        {
            _light.intensity = Mathf.Clamp01(Mathf.Sin(_timeController.DayProgress / 0.5f * Mathf.PI));

            transform.eulerAngles = _baseRotation
                + (_timeController.DayProgress * 360 * _rotationDirection)
                + (Mathf.Sin(_timeController.DayProgress * Mathf.PI * 2) * _sinRotationStrength * _sinRotationDirection);
        }
    }
}
