using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Lunaculture.Placement
{
    public class BuildModeController : MonoBehaviour
    {
        [SerializeField]
        public UnityEvent<string?> _modeChanged = null!;

        [SerializeField]
        private string[] _menus = Array.Empty<string>();

        private string? _currentMenu;
        
        public void ToggleBuildMode(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed || _menus.Length == 0)
                return;

            var nextMenu = string.Empty;
            if (_currentMenu == null)
                nextMenu = _menus[0];
            else
            {
                var nextIndex = Array.IndexOf(_menus, _currentMenu);
                if (nextIndex == -1)
                    throw new InvalidOperationException("Missing menu");

            }
            
        }

        public void ExitBuildMode(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
                return;
            
            _modeChanged.Invoke(null);
        }
    }
}