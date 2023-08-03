using UnityEngine;

namespace GameAssets.Scripts.Utils
{
    public class Lifetime : MonoBehaviour
    {
        [SerializeField] private float lifetime;

        private void Start()
        {
            Destroy(gameObject,lifetime);
        }
    }
}
