using Cinemachine;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using Lunaculture.Cameras;
using Lunaculture.Objectives;
using Lunaculture.UI;
using UnityEngine;

namespace Lunaculture.GameTime
{
    public class SpeedToNextWeekController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera = null!;

        [SerializeField]
        private CameraZoomController _cameraZoomController = null!;

        [SerializeField]
        private TimeController _timeController = null!;

        [SerializeField]
        private ObjectiveService _objectiveService = null!;

        [SerializeField]
        private MenuPopupController _menuPopupController = null!;

        [SerializeField]
        private bool _hijackCamera = true;

        private Tween<Vector3>? _activeCamera;
        private Tween<float>? _activeSpeed;

        private bool _timeSpeedUp = false;
        private float _oldGameSpeed = 1f;
        private Vector3 _oldCameraPosition = Vector3.one;

        private CinemachineTransposer _transposer = null!;
        private CinemachineVirtualCamera _lastCamera = null!;

        private void Update()
        {
            if (_lastCamera != _virtualCamera)
            {
                _lastCamera = _virtualCamera;
                _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            }

            if (_timeSpeedUp)
                _cameraZoomController.Disable(true);
        }

        private void Start()
        {
            _objectiveService.OnObjectiveProgress += ObjectiveService_OnObjectiveProgress;
            _timeController.OnWeekChange += TimeController_OnWeekChange;
        }

        private void TimeController_OnWeekChange(WeekChangeEvent obj)
        {
            if (_timeSpeedUp)
            {
                _timeSpeedUp = false;

                if (_hijackCamera)
                {
                    _activeCamera.AsNull()?.Cancel();
                    _activeCamera = gameObject
                        .TweenValueVector3(_oldCameraPosition, 2.5f, val => _transposer.m_FollowOffset = val)
                        .SetFrom(_transposer.m_FollowOffset)
                        .SetUseUnscaledTime(true)
                        .SetOnComplete(() =>
                        {
                            _cameraZoomController.Disable(false);
                            _activeCamera = null;
                        })
                        .SetEase(EaseType.QuartOut);
                }

                _activeSpeed.AsNull()?.Cancel();
                _timeController.GameSpeed = _menuPopupController.MenuOpen ? 0f : 1f;
            }
        }

        private void ObjectiveService_OnObjectiveProgress(float progress)
        {
            if (progress != 1) return;

            _timeSpeedUp = true;

            if (_hijackCamera)
            {
                _oldCameraPosition = _transposer.m_FollowOffset;

                _activeCamera.AsNull()?.Cancel();
                _activeCamera = gameObject
                    .TweenValueVector3(new(0, 5f, -10f), 2.5f, val => _transposer.m_FollowOffset = val)
                    .SetFrom(_oldCameraPosition)
                    .SetUseUnscaledTime(true)
                    .SetOnComplete(() => _activeCamera = null)
                    .SetEase(EaseType.QuartIn);
            }

            _activeSpeed.AsNull()?.Cancel();
            _activeSpeed = gameObject
                .TweenValueFloat(10f, 7.5f, val => _timeController.GameSpeed = val)
                .SetFrom(_oldGameSpeed)
                .SetUseUnscaledTime(true)
                .SetOnComplete(() => _activeSpeed = null)
                .SetEase(EaseType.QuartIn);
        }

        private void OnDestroy()
        {
            
        }
    }
}
