using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture.UI
{
    public class MenuPopupController : MonoBehaviour
    {
        public bool MenuOpen => currentlyOpenMenu != null;

        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;

        private MenuBase? currentlyOpenMenu;
        private int lastMenuOpenFrame;

        public bool TryOpenMenu(MenuBase menu)
        {
            if (currentlyOpenMenu != null)
                return false;
            
            currentlyOpenMenu = menu;
            menu.gameObject.SetActive(true);

            gameUIInterconnect.PauseController.Paused = menu.PausesGame;

            lastMenuOpenFrame = Time.frameCount;

            return true;
        }

        public void CloseOpenMenu(InputAction.CallbackContext context)
        {
            if (!context.performed || lastMenuOpenFrame == Time.frameCount) return;

            CloseOpenMenu();
        }

        public void CloseOpenMenu()
        {
            if (currentlyOpenMenu == null) return;

            currentlyOpenMenu.gameObject.SetActive(false);
            currentlyOpenMenu = null;
            gameUIInterconnect.PauseController.Paused = false;
        }
    }
}
