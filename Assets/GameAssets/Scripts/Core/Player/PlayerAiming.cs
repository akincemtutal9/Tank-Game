using GameAssets.Scripts.Input;
using Unity.Netcode;
using UnityEngine;
namespace GameAssets.Scripts.Core.Player
{
    public class PlayerAiming : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Transform turretTransform;

        private Vector2 _previousLookDirection;
        
        public override void OnNetworkSpawn()// Start 
        {
            if (!IsOwner) {return;}
            inputReader.AimEvent += HandleAim;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner) {return;}
            inputReader.AimEvent -= HandleAim;

        }
        // Update is called once per frame
        private void LateUpdate()
        {
            if (!IsOwner) {return;}

            var aimWorldPosition = (Vector2) Camera.main.ScreenToWorldPoint(_previousLookDirection);
            var turretPosition = (Vector2) turretTransform.position;
            turretTransform.up = new Vector2(
                aimWorldPosition.x - turretPosition.x,
                aimWorldPosition.y - turretPosition.y
            );
        }
        private void HandleAim(Vector2 turretLookDirection)
        {
            _previousLookDirection = turretLookDirection;
        }
    }
}
