using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lunaculture.UI.Dialog
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private Button? yesButton = null!;
        [SerializeField] private Button? noButton = null!;
        [SerializeField] private TextMeshProUGUI? boxText = null!;
        [SerializeField] private MenuBase? menuBase = null!;
        [SerializeField] private MenuPopupController? menuPopupController = null!;

        private MenuBase? parentMenu = null!;

        public void OpenBox(MenuBase? parentMenu, string text, Action? onYes, Action? onNo)
        {
            this.parentMenu = parentMenu;

            boxText.text = text;

            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();

            yesButton.onClick.AddListener(() => InvokeActionAndClose(onYes));
            noButton.onClick.AddListener(() => InvokeActionAndClose(onNo));

            menuPopupController.CloseOpenMenu();
            menuBase.TryOpenMenu();
        }

        private void InvokeActionAndClose(Action? action)
        {
            Close();
            action?.Invoke();
        }

        private void Close()
        {
            if (parentMenu != null)
            {
                menuPopupController.CloseOpenMenu();
                parentMenu.TryOpenMenu();
            }
        }
    }
}
