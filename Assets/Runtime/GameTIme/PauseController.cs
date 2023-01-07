using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunaculture.GameTime
{
    public class PauseController : MonoBehaviour
    {
        public bool Paused
        {
            get => paused;
            set
            {
                if (value && !paused)
                {
                    previousGameSpeed = timeController.GameSpeed;
                    timeController.GameSpeed = 0f;
                }
                else if (!value && paused)
                {
                    timeController.GameSpeed = previousGameSpeed;
                }

                paused = value;
            }
        }

        [SerializeField] private TimeController timeController = null!;

        private bool paused;
        private float previousGameSpeed;
    }
}
