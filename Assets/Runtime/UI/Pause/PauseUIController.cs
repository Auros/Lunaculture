using Lunaculture.UI.Dialog;
using UnityEngine;

namespace Lunaculture.UI.Pause
{
    public class PauseUIController : MonoBehaviour
    {
        private const string LEAVE_TEXT = "Are you sure you want to leave?\nAll progress will be lost!\n\n(Seriously, we don't have a save/load system.)";

        [SerializeField] private DialogBoxController? dialogBoxController = null!;
        [SerializeField] private SceneTransitionController? sceneTransitionController = null!;
        [SerializeField] private MenuBase? pauseMenu = null!;

        public void ShowMenuPrompt()
        {
            dialogBoxController.OpenBox(pauseMenu, LEAVE_TEXT, sceneTransitionController.GoToMenu, null);
        }

        public void ShowQuitPrompt()
        {
            dialogBoxController.OpenBox(pauseMenu, LEAVE_TEXT, () => Application.Quit(), null);
        }
    }
}
