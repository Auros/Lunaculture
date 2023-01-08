using TMPro;
using UnityEngine;

namespace Lunaculture.UI.MainMenu
{
    public class VersionTextController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI versionText;

        private void Start()
            => versionText.text = Application.version;
    }
}
