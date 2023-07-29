using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class JoinServer : MonoBehaviour
{
    [SerializeField] private Button joinClientButton;
        
    void Start()
    {
        HandleJoinClient();
    }

    private void HandleJoinClient()
    {
        joinClientButton.OnClickAsObservable().Subscribe(_ =>
        {
            NetworkManager.Singleton.StartClient();
        }).AddTo(gameObject);
    }
}
