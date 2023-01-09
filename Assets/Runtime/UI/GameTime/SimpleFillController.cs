using UnityEngine;
using UnityEngine.UI;

namespace Lunaculture.UI.GameTime
{
    public class SimpleFillController : MonoBehaviour
    {
        public float Fill
        {
            get => fillTransform.anchorMax.x;
            set => fillTransform.anchorMax = new Vector2(Mathf.Clamp01(value), 1);
        }

        public Color Color
        {
            get => image.color;
            set => image.color = value;
        }

        [SerializeField] private RectTransform fillTransform = null!;
        [SerializeField] private Image image = null!;
    }
}
