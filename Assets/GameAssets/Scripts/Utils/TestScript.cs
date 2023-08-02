using GameAssets.Scripts.Input;
using UnityEngine;

namespace GameAssets.Scripts.Utils
{
    public class TestScript : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        void Start()
        {
            inputReader.MoveEvent += HandleMove;
        }

        private void OnDestroy()
        {
            inputReader.MoveEvent -= HandleMove;
        }
        private void HandleMove(Vector2 movement)
        {
            Debug.Log(movement);
        }
    }
}
