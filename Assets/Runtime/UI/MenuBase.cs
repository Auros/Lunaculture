using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture.UI
{
    public class MenuBase : MonoBehaviour
    {
        [field: SerializeField]
        public bool PausesGame { get; private set; }

        [SerializeField] private MenuPopupController menuPopupController = null!;

        private bool openMenu = false;

        public void ToggleMenu(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (openMenu)
            {
                menuPopupController.CloseOpenMenu();
                openMenu = false;
            }
            else
            {
                openMenu = menuPopupController.TryOpenMenu(this);
            }
        }
    }
}
