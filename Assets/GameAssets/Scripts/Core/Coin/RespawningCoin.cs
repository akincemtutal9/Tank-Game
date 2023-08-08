using System;
using UnityEngine;

namespace GameAssets.Scripts.Core.Coin
{
    public class RespawningCoin : Coin
    {
        public event Action<RespawningCoin> OnCollected;

        private Vector3 previousPosition;
        private void Update()
        {
            if (previousPosition != transform.position)
            {
                Show(true);
            }

            previousPosition = transform.position;
        }
        public override int Collect()
        {
            if (!IsServer)
            {
                Show(false);
                return 0;
            }
            if (alreadyConnected) { return 0; }
            alreadyConnected = true;
            OnCollected?.Invoke(this);
            return coinValue;
        }
        public void Reset()
        {
            alreadyConnected = false;
        }
    }
}
