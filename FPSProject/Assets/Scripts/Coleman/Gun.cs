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
    public GameObject playerOrientation;
    public GameObject WeaponHolder;
    public GameObject hitMarker;
    public PhotonView PV;
    public Text ammoCounter;
    public bool settingsOpen = false;
    public bool equipped;
    Animator animator;
    AudioSource[] sounds;
    AudioSource gunshot;
    AudioSource reload;

    private RPC_Functions rpcFunc;

    float timeSinceLastShot;

    private void Start()
    {
        if (transform.parent != null)
        {
            equipped = true;            
            ammoCounter = GameObject.Find("AmmoCounter").GetComponent<Text>();
            hitMarker = GameObject.Find("HitMarker");
            hitMarker.SetActive(false);
            gunData.currentAmmo = gunData.magSize;
            PlayerShoot.shootInput += Shoot;
            PlayerShoot.reloadInput += ReloadInit;
            animator = GetComponent<Animator>();
            sounds = GetComponents<AudioSource>();
            gunshot = sounds[0];
            reload = sounds[1];
        } else
        {
            equipped = false;
        }
    }
    void Awake()
    {
        if (transform.parent != null)
        {
            player = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            PV = player.GetComponent<PhotonView>();

            rpcFunc = GetComponentInParent<RPC_Functions>();
            rpcFunc.gunShot = GetComponents<AudioSource>()[0];
            rpcFunc.reload = GetComponents<AudioSource>()[1];
        }
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
            PV.RPC("triggerAnim", RpcTarget.OthersBuffered, "Reload");
        }
    }

    private bool canShoot()
    {
        if (!gunData.isReloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && !settingsOpen && equipped)
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
                    if (!gunData.isShotgun)
                    {
                        if (Physics.Raycast(playerOrientation.transform.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                        {
                            if (hitInfo.collider.tag == "Player")
                            {
                                Debug.Log("Hit");
                                StartCoroutine(playerHit());
                                hitInfo.transform.GetComponent<PhotonView>().RPC("DamagePlayer", RpcTarget.AllBuffered, gunData.damage);
                            }
                            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                            Debug.Log(hitInfo);
                            damageable?.Damage(gunData.damage);
                        }
                    }
                    else
                    {
                        for (var i = 0; i < 12; i++)
                        {
                            Vector2 localOffset = playerOrientation.transform.position;
                            float randomX = Random.Range(-1.0f, 1.0f);
                            float randomY = Random.Range(-1.0f, 1.0f);
                            localOffset.y += randomY;
                            localOffset.x += randomX;
                            if (Physics.Raycast(localOffset, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                            {
                                if (hitInfo.collider.tag == "Player")
                                {
                                    Debug.Log("Hit");
                                    StartCoroutine(playerHit());
                                    hitInfo.transform.GetComponent<PhotonView>().RPC("DamagePlayer", RpcTarget.AllBuffered, gunData.damage);
                                }
                                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                                Debug.Log(hitInfo);
                                damageable?.Damage(gunData.damage);
                            }
                        }
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
        Debug.DrawRay(playerOrientation.transform.position, transform.forward);

        if (transform.parent != null)
        {
            ammoCounter = GameObject.Find("AmmoCounter").GetComponent<Text>();
            if (gunData.reserveAmmo == -1) ammoCounter.text = gunData.currentAmmo.ToString() + "/\u221e";
            else ammoCounter.text = gunData.currentAmmo.ToString() + "/" + gunData.reserveAmmo.ToString();
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    private void OnGunShot()
    {
        if (this.gameObject.activeSelf) {
            flash.Play();
            gunshot.Play();
            animator.SetTrigger("Shoot");
            PV.RPC("triggerAnim", RpcTarget.OthersBuffered, "Shoot");
        }
    }

    private IEnumerator playerHit()
    {
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(.25f);
        hitMarker.SetActive(false);
    }
}
