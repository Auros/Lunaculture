using TMPro;
using UnityEngine;

namespace Lunaculture.UI.Dialog
{
    public class LoreBoxController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI? boxText = null!;
        [SerializeField] private MenuBase? menuBase = null!;
        [SerializeField] private MenuPopupController? menuPopupController = null!;

        public void DumpLore(string lore)
        {
            boxText.text = lore;

            menuPopupController.CloseOpenMenu();
            menuBase.TryOpenMenu();
        }

        public void Close() => menuPopupController.CloseOpenMenu();
    }
}
