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
        [SerializeField] private GameObject muzzleFlash;
        [SerializeField] private Collider2D playerCollider;
        [Header("Settings")]
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float fireRate;
        [SerializeField] private float muzzleFlashDuration;
        
        private bool shouldFire;
        private float previousFireTime;
        private float muzzleFlashTimer;
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
            if (muzzleFlashTimer > 0f)
            {
                muzzleFlashTimer -= Time.deltaTime;
                if (muzzleFlashTimer <= 0f)
                {
                    muzzleFlash.SetActive(false);
                }
            }
            
            if(!IsOwner){ return; } // Owner değilsek başkasının kodlarını kullanamıyoruz dogal olarak
            if (!shouldFire) { return; } // Tuşa basmıyorsak bulletları sıkmıyor

            if (Time.time < (1 / fireRate) + previousFireTime)
            {
                return;
            }
            PrimaryFireServerRpc(projectileSpawnPoint.position,projectileSpawnPoint.up); // Bu taraf hem clientta bir clientProj hemde serverProj oluşturuyor
            SpawnDummyProjectile(projectileSpawnPoint.position,projectileSpawnPoint.up); // Attıgımız projectile bizim tarafta delaysız gözüksün diye böyle
            
            previousFireTime = Time.time;
        }
        [ServerRpc]
        private void PrimaryFireServerRpc(Vector3 spawnPosition,Vector3 direction)
        {
            GameObject projectile = Instantiate(serverProjectilePrefab, spawnPosition, Quaternion.identity);
            projectile.transform.up = direction;
            
            Physics2D.IgnoreCollision(playerCollider,projectile.GetComponent<CircleCollider2D>());
            
            if (projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = projectileSpeed * rb.transform.up;
            }
            SpawnDummyProjectileClientRpc(spawnPosition,direction);// Burda bişey oluşmayacak aslında bizim tarafımızda ama başkasının sıktıgı mermiye burda bir görüntü atamış oluyoruz

        }
        
        // Input Readerdan Input Actionstaki butona basıyor muyuz diye bakıyor burası 
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
            muzzleFlash.SetActive(true);
            muzzleFlashTimer = muzzleFlashDuration;
            
            GameObject projectile = Instantiate(clientProjectilePrefab, spawnPosition, Quaternion.identity);
            projectile.transform.up = direction;
            
            Physics2D.IgnoreCollision(playerCollider,projectile.GetComponent<CircleCollider2D>());

            if (projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = projectileSpeed * rb.transform.up;
            }
        }
    }
}
