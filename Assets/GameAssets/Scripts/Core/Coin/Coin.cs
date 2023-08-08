using Unity.Netcode;
using UnityEngine;

namespace GameAssets.Scripts.Core.Coin
{
    public abstract class Coin : NetworkBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        protected int coinValue = 10;
        protected bool alreadyConnected;

        public abstract int Collect();

        public void SetValue(int value)
        {
            coinValue = value;
        }

        public int GetValue()
        {
            return coinValue;
        }

        protected void Show(bool show)
        {
            spriteRenderer.enabled = show;
        }
    
    }
}
