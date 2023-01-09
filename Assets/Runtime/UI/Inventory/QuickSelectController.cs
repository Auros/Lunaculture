using Lunaculture.Player.Inventory;
using Lunaculture.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture
{
    public class QuickSelectController : MonoBehaviour
    {
        [SerializeField] private GameUIInterconnect? gameUIInterconnect = null!;

        private InventoryService? inventoryService = null!;

        public void OnSelectItem0(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(0);
        }

        public void OnSelectItem1(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(1);
        }

        public void OnSelectItem2(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(2);
        }

        public void OnSelectItem3(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(3);
        }

        public void OnSelectItem4(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(4);
        }

        public void OnSelectItem5(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(5);
        }

        public void OnSelectItem6(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(6);
        }

        public void OnSelectItem7(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(7);
        }

        public void OnSelectItem8(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(8);
        }

        public void OnSelectItem9(InputAction.CallbackContext context)
        {
            if (context.performed) QuickSelectItem(9);
        }

        private void Start()
        {
            inventoryService = gameUIInterconnect.InventoryService;
        }

        private void QuickSelectItem(int idx)
        {
            inventoryService.SelectItem(inventoryService.Inventory[idx], true);
        }
    }
}
