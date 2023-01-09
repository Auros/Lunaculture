using UnityEngine;

namespace Lunaculture.UI.MainMenu
{
    public class ResetGameSpeedController : MonoBehaviour
    {
        private void Start() => Time.timeScale = 1.0f;
    }
}
