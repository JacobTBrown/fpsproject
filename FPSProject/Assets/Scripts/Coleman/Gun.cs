using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private ParticleSystem flash;
    public Text ammoCounter;
    Animator animator;
    AudioSource[] sounds;
    AudioSource gunshot;
    AudioSource reload;

    float timeSinceLastShot;
    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += ReloadInit;
        animator = GetComponent<Animator>();
        sounds = GetComponents<AudioSource>();
        gunshot = sounds[0];
        reload = sounds[1];
    }

    private IEnumerator Reload()
    {
        gunData.isReloading = true;
        reload.Play();
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.isReloading = false;
    }

    public void ReloadInit()
    {
        if(!gunData.isReloading && gunData.magSize != gunData.currentAmmo)
        {
            StartCoroutine(Reload());
            animator.SetTrigger("Reload");
        }
    }

    private bool canShoot()
    {
        if (!gunData.isReloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (canShoot())
            {
                if (Physics.Raycast(muzzle.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gunData.damage);
                }
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }    
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(muzzle.position, muzzle.forward);
        ammoCounter.text = gunData.currentAmmo.ToString() + "/" + gunData.reserveAmmo.ToString();
    }

    private void OnGunShot()
    {
        flash.Play();
        gunshot.Play();
        animator.SetTrigger("Shoot");
    }
}
