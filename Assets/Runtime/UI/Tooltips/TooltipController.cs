using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture.UI.Tooltips
{
    public class TooltipController : MonoBehaviour
    {
        [SerializeField] private RectTransform? _tooltipTransform = null!;
        [SerializeField] private RectTransform? _parentTransform = null!;
        [SerializeField] private TextMeshProUGUI? _nameText = null!;
        [SerializeField] private TextMeshProUGUI? _textText = null!;

        public void OnTooltipMovement(InputAction.CallbackContext context)
        {
            _tooltipTransform.position = context.ReadValue<Vector2>();
        }

        public void ShowTooltip(TooltipSource source) => ShowTooltip(source.TooltipName, source.TooltipText);

        public void ShowTooltip(string name, string text)
        {
            _tooltipTransform.gameObject.SetActive(true);
            _nameText.text = name;
            _textText.text = text;
        }

        public void CloseTooltip()
        {
            _tooltipTransform.gameObject.SetActive(false);
        }
    }
}
