using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GunData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private ParticleSystem flash;
    public GameObject player;
    public PhotonView PV;
    public Text ammoCounter;
    public bool settingsOpen = false;
    Animator animator;
    AudioSource[] sounds;
    AudioSource gunshot;
    AudioSource reload;

    float timeSinceLastShot;

    private void Start()
    {
        ammoCounter = GameObject.Find("AmmoCounter").GetComponent<Text>();
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += ReloadInit;
        animator = GetComponent<Animator>();
        sounds = GetComponents<AudioSource>();
        gunshot = sounds[0];
        reload = sounds[1];
    }
    void Awake()
    {
        player = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        PV = player.GetComponent<PhotonView>();
    }

    private IEnumerator Reload()
    {
        gunData.isReloading = true;
        reload.Play();
        yield return new WaitForSeconds(gunData.reloadTime);
        if (gunData.reserveAmmo != -1) gunData.reserveAmmo -= gunData.magSize - gunData.currentAmmo;
        gunData.currentAmmo = gunData.magSize;
        gunData.isReloading = false;
    }

    private void OnDisable() => gunData.isReloading = false;

    public void ReloadInit()
    {
        if(!gunData.isReloading && gunData.magSize != gunData.currentAmmo && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
            animator.SetTrigger("Reload");
        }
    }

    private bool canShoot()
    {
        if (!gunData.isReloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && !settingsOpen)
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
        if(PV.IsMine)
        {
            if (gunData.currentAmmo > 0)
            {
                if (canShoot())
                {
                    if (Physics.Raycast(muzzle.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                    {
                        if (hitInfo.collider.tag == "Player")
                        {
                            Debug.Log("Hit");
                            hitInfo.transform.GetComponent<PhotonView>().RPC("DamagePlayer", RpcTarget.AllBuffered, gunData.damage);
                        }
                        IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                        Debug.Log(hitInfo);
                        damageable?.Damage(gunData.damage);
                    }
                    gunData.currentAmmo--;
                    timeSinceLastShot = 0;
                    OnGunShot();
                }
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(muzzle.position, muzzle.forward);
     
        if(gunData.reserveAmmo == -1) ammoCounter.text = gunData.currentAmmo.ToString() + "/\u221e";
        else ammoCounter.text = gunData.currentAmmo.ToString() + "/" + gunData.reserveAmmo.ToString();
    }

    private void OnGunShot()
    {
        if (this.gameObject.activeSelf) {
            flash.Play();
            gunshot.Play();
            animator.SetTrigger("Shoot");
        }
    }
}
