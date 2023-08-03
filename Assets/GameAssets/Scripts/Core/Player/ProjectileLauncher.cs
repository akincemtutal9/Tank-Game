using GameAssets.Scripts.Input;
using Unity.Netcode;
using UnityEngine;

namespace GameAssets.Scripts.Core.Player
{
    public class ProjectileLauncher : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private GameObject serverProjectilePrefab;
        [SerializeField] private GameObject clientProjectilePrefab;
        [Header("Settings")]
        [SerializeField] private float projectileSpeed;

        private bool shouldFire;
        public override void OnNetworkSpawn()// Start 
        {
            if (!IsOwner) {return;}
            inputReader.PrimaryFireEvent += HandlePrimaryFire;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner) {return;}
            inputReader.PrimaryFireEvent -= HandlePrimaryFire;
        }
        private void Update()
        {
            if(!IsOwner){ return; } // Owner değilsek başkasının kodlarını kullanamıyoruz dogal olarak
            if (!shouldFire) { return; } // Tuşa basmıyorsak bulletları sıkmıyor
            
            PrimaryFireServerRpc(projectileSpawnPoint.position,projectileSpawnPoint.up); // Bu taraf hem clientta bir clientProj hemde serverProj oluşturuyor
            SpawnDummyProjectile(projectileSpawnPoint.position,projectileSpawnPoint.up); // Attıgımız projectile bizim tarafta delaysız gözüksün diye böyle
        }
        [ServerRpc]
        private void PrimaryFireServerRpc(Vector3 spawnPosition,Vector3 direction)
        {
            GameObject projectile = Instantiate(serverProjectilePrefab, spawnPosition, Quaternion.identity);
            projectile.transform.up = direction;
            
            // Burda bişey oluşmayacak aslında bizim tarafımızda ama başkasının sıktıgı mermiye burda bir görüntü atamış oluyoruz
            SpawnDummyProjectileClientRpc(spawnPosition,direction);
        }
        private void HandlePrimaryFire(bool shouldFire)
        {
            this.shouldFire = shouldFire;
        }
        
        // Burda ownersak eğer bi obje oluşmayacak yani bu kod bizden hariç diğer clientların atacagı projectileı ilgilendiriyor
        [ClientRpc]
        private void SpawnDummyProjectileClientRpc(Vector3 spawnPosition, Vector3 direction)
        {
            if (IsOwner) { return; }
            SpawnDummyProjectile(spawnPosition,direction);
        }
        
        //Bir tane client Projectile atıyor görüntülü olandan atıyor 
        private void SpawnDummyProjectile(Vector3 spawnPosition, Vector3 direction)
        {
            GameObject projectile = Instantiate(clientProjectilePrefab, spawnPosition, Quaternion.identity);
            projectile.transform.up = direction;
        }
    }
}
