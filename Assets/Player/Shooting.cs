using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public  GameObject TargetGun;
    public float range = 100f;
    private Animator anim;
    public float BulletDamage;
    public float ReloadTime;
    public float MaxAmmo = 100f;
    public float CurrentAmmo;
    public ParticleSystem MuzzleFlash;
    public float FireRate = 2f;
    private LayerMask ignorePlayerLayerMask;
    private bool isReloading;
    private bool isShooting;
    private float lastShotTime;

    private void Start()
    {
        anim = GetComponent<Animator>();
        CurrentAmmo = MaxAmmo;

        int playerLayer = LayerMask.NameToLayer("Player");
        ignorePlayerLayerMask = ~LayerMask.GetMask(LayerMask.LayerToName(playerLayer));

        MuzzleFlash.Stop();
    }

    private void Update()
    {
        // Check if the TargetGun is active before allowing shooting and reloading
        if (TargetGun.activeSelf)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !isReloading && !isShooting)
            {
                if (CurrentAmmo > 0)
                {
                    if (Time.time > lastShotTime + FireRate)
                    {
                        StartCoroutine(Shoot());
                    }
                }
                else
                {
                    // Force reload when the current ammo is 0
                    StartCoroutine(Reload());
                }
            }
        }
    }


    private IEnumerator Shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        MuzzleFlash.Play();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, ignorePlayerLayerMask))
        {
            if (hit.collider.CompareTag("Enemy") )
            {

                Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);

                EnemyHealthBar enemyHealthBar = hit.transform.GetComponent<EnemyHealthBar>();
                if (enemyHealthBar != null)
                {
                    enemyHealthBar.TakeDamageFromPlayer(BulletDamage);
                }
            }
        }

        
        CurrentAmmo--;
        yield return new WaitForSeconds( FireRate);
        isShooting = false;
    }

    private IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;
        anim.SetTrigger("Reload"); // Trigger the "Reload" animation
        MuzzleFlash.Stop(); // Stop the muzzle flash when reloading
        yield return new WaitForSeconds(ReloadTime);
        CurrentAmmo = MaxAmmo;
        isReloading = false;

        Debug.Log("Reloaded!");
    }


}
