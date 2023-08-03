using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GameAssets.Scripts.Utils
{
    public class DestroySelfOnContact : MonoBehaviour
    {
        private void Start()
        {
            HandleContact();
        }

        private void HandleContact()
        {
            this.OnTriggerEnter2DAsObservable().Subscribe(_ =>
            {
                Destroy(gameObject);
            }).AddTo(gameObject);
        }
    }
}
