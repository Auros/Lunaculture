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

        private Camera _camera;

        private bool tooltipActive = false;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void OnTooltipMovement(InputAction.CallbackContext context)
        {
            _tooltipTransform.position = context.ReadValue<Vector2>();
        }

        private void OnTooltipEnter(TooltipSource source)
        {
            _tooltipTransform.gameObject.SetActive(true);
            tooltipActive = true;
            _nameText.text = source.TooltipName;
            _textText.text = source.TooltipText;
        }

        private void OnTooltipExit(TooltipSource source)
        {
            tooltipActive = false;
            _tooltipTransform.gameObject.SetActive(false);
        }
    }
}
