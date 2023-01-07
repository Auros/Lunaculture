using UnityEngine;

namespace Lunaculture.UI.GameTime
{
    public class SimpleFillController : MonoBehaviour
    {
        public float Fill
        {
            get => fillTransform.anchorMax.x;
            set => fillTransform.anchorMax = new(Mathf.Clamp01(value), 1);
        }

        [SerializeField] private RectTransform fillTransform;
    }
}
