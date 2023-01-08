using Cinemachine;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture.Cameras
{
    [ExecuteAlways]
    public class CameraZoomController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera = null!;

        [SerializeField]
        private AnimationCurve _yFollowOffsetCurve = null!;

        [SerializeField]
        private AnimationCurve _zFollowOffsetCurve = null!;

        [SerializeField, Min(0.01f)]
        private float _scrollSpeed = 0.05f;

        private Tween<float>? _active;

        [field: SerializeField]
        [field: Range(0, 1)]
        public float CurrentValue { get; set; }

        private CinemachineTransposer _transposer = null!;
        private CinemachineVirtualCamera _lastCamera = null!;

        private void Update()
        {
            if (_lastCamera != _virtualCamera)
            {
                _lastCamera = _virtualCamera;
                _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            }

            var value = CurrentValue;
            var y = _yFollowOffsetCurve.Evaluate(value);
            var z = _zFollowOffsetCurve.Evaluate(value);
            _transposer.m_FollowOffset = new Vector3(0f, y, z);
        }

        public void Disable(bool value)
        {
            enabled = !value;
        }

        [UsedImplicitly]
        public void OnScroll(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
                return;

            var value = ctx.ReadValue<float>();
            value = CurrentValue - _scrollSpeed * Time.deltaTime * value;
            if (value < 0)
                value = 0;

            if (value > 1)
                value = 1;

            _active.AsNull()?.Cancel();
            _active = gameObject
                .TweenValueFloat(value, 0.5f, val => CurrentValue = val)
                .SetFrom(CurrentValue)
                .SetOnComplete(() => _active = null)
                .SetEase(EaseType.QuartOut);
        }
    }
}