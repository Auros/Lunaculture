using UnityEngine;

namespace Lunaculture.UI.GameTime
{
    public class SimpleFillController : MonoBehaviour
    {
        public float Fill
        {
            get => fillTransform.anchorMax.x;
            set => fillTransform.anchorMax = new Vector2(Mathf.Clamp01(value), 1);
        }

        [SerializeField] private RectTransform fillTransform = null!;
    }
}
