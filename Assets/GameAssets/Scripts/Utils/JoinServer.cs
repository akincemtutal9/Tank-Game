using Unity.Netcode;
using UnityEngine;

namespace GameAssets.Scripts.Utils
{
    public class JoinServer : MonoBehaviour
    {
        public void Join()
        {
            // This is working after if game is hosted before
            NetworkManager.Singleton.StartClient();
        }

        public void Host()
        {
            NetworkManager.Singleton.StartHost();
        }
    }
}