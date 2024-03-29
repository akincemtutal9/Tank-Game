using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameAssets.Scripts.Input
{
    [CreateAssetMenu(fileName ="New Input Reader", menuName = "Input/Input Reader")]
    public class InputReader : ScriptableObject, Controls.IPlayerActions
    {
        public event Action<bool> PrimaryFireEvent;
        public event Action<Vector2> MoveEvent;
        public event Action<Vector2> AimEvent;

        private Controls controls;
        private void OnEnable()
        {
            if (controls == null)
            {
                controls = new Controls();
                controls.Player.SetCallbacks(this);
            }
            
            controls.Player.Enable();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnPrimaryFire(InputAction.CallbackContext context)
        {
            if(context.performed) {PrimaryFireEvent?.Invoke(true);}
            else if(context.canceled) {PrimaryFireEvent?.Invoke(false);}
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            AimEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }
}
