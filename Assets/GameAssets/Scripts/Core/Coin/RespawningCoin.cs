namespace GameAssets.Scripts.Core.Coin
{
    public class RespawningCoin : Coin
    {
        public override int Collect()
        {
            if (!IsServer)
            {
                Show(false);
                return 0;
            }

            if (alreadyConnected)
            {
                return 0;
            }

            alreadyConnected = true;
            return coinValue;
        }
    }
}
