using Lunaculture.GameTime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture.UI.Pause
{
    public class PauseController : MonoBehaviour
    {
        public bool Paused { get; private set; } = false;

        [SerializeField] private GameUIInterconnect gameUIInterconnect;

        private TimeController timeController;
        private float previousGameSpeed = 1f;

        public void OnPause(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            TogglePause(!Paused);
        }

        public void TogglePause(bool pause)
        {
            Paused = pause;

            gameObject.SetActive(pause);

            if (pause)
            {
                previousGameSpeed = timeController.GameSpeed;
                timeController.GameSpeed = 0f;
            }
            else
            {
                timeController.GameSpeed = previousGameSpeed;
            }
        }

        private void OnEnable()
            => timeController = gameUIInterconnect.TimeController;
    }
}
