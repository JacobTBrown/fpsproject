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
    public GameObject playerOrientation;
    public GameObject WeaponHolder;
    public GameObject hitMarker;
    public Collider thisCollider;
    public PhotonView PV;
    public Text ammoCounter;
    public bool settingsOpen = false;
    public bool equipped;
    Animator animator;
    public AudioClip gunshot;
    public AudioClip reload;
    public AudioSource audioSource;

    //private RPC_Functions rpcFunc;

    float timeSinceLastShot;

    private void Start()
    {
<<<<<<< HEAD
        PV = GetComponentInParent<PhotonView>();
        if (equipped && PV.IsMine)
        {          
            ammoCounter = GameObject.Find("AmmoCounter").GetComponent<Text>();
            playerOrientation = GetComponentInParent<Camera>().gameObject;
            WeaponHolder = GetComponentInParent<WeaponSway>().gameObject;
            thisCollider = gameObject.transform.parent.parent.parent.GetComponentInChildren<CapsuleCollider>();
            audioSource = GetComponent<AudioSource>();
            gunData.currentAmmo = gunData.magSize;
            gunData.reserveAmmo = gunData.maxReserveAmmo;
            var shoot = GetComponentInParent<PlayerShoot>();
            shoot.shootInput += Shoot;
            shoot.reloadInput += ReloadInit;
            //PlayerShoot.shootInput += Shoot;
            //PlayerShoot.reloadInput += ReloadInit;
            animator = GetComponent<Animator>();

            hitMarker = GameObject.Find("HitMarker");
            if(hitMarker) hitMarker.SetActive(false);
=======
        //Debug.Log("Gun.cs start");
        //Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        rpcFunc = GetComponentInParent<RPC_Functions>();
        rpcFunc.gunShot = GetComponents<AudioSource>()[0];
        rpcFunc.reload = GetComponents<AudioSource>()[1];
        if (!this)
        {
            Debug.Log("gun.cs is null ");
        }
        if (transform.parent != null) // && PV.IsMine)
        {
            if (ammoCounter)
            {
                Debug.Log("ammo counter was already set, don't run this again");
                return;
            }
            equipped = true;            
            ammoCounter = GameObject.Find("AmmoCounter").GetComponent<Text>();
            hitMarker = GameObject.Find("HitMarker");
            if (hitMarker) { hitMarker.SetActive(false); Debug.Log("set ur hitmarker"); };
            PlayerShoot.shootInput += Shoot;
            PlayerShoot.reloadInput += ReloadInit;
            animator = GetComponent<Animator>();
            sounds = GetComponents<AudioSource>();
            gunshot = sounds[0];
            reload = sounds[1];
            Debug.Log("Gun.cs exited start with reload: " + reload.name);
        } else
        {
           Debug.Log("gun.cs failed");
            equipped = false;
        }
    }
    void Awake()
    {
        this.gameObject.SetActive(true);
        Debug.Log("Gun.cs awake");
        if (player)
        {
            Debug.Log("player was already set: " + player.GetComponent<PhotonView>().ViewID.ToString());
        }
        if (transform.parent != null)
        {
            
            player = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            PV = player.GetComponent<PhotonView>();
            Debug.Log("player was set: " + player.GetComponent<PhotonView>().ViewID.ToString());
            //Debug.Log("player's pv: " + PV.ViewID);

            //moving RPCs to start() - Kassad November 2018
            //https://forum.photonengine.com/discussion/2300/solved-received-rpc-photonview-does-not-exist

>>>>>>> Blake_Brooks(Current)
        }

    }

    private IEnumerator Reload()
    {
        gunData.isReloading = true;
        audioSource.clip = reload;
        audioSource.Play();
        yield return new WaitForSeconds(gunData.reloadTime);
        if (gunData.reserveAmmo != -1) gunData.reserveAmmo -= gunData.magSize - gunData.currentAmmo;
        gunData.currentAmmo = gunData.magSize;
        gunData.isReloading = false;
    }

    private void OnDisable() => gunData.isReloading = false;

    public void ReloadInit()
    {
<<<<<<< HEAD
        if (this.gameObject.activeSelf) {
            if(!gunData.isReloading && gunData.magSize != gunData.currentAmmo) {
                StartCoroutine(Reload());
                animator.SetTrigger("Reload");
                switch(gunData.name) {
                    case "M1911":
                        PV.RPC("TriggerAnimPistol", RpcTarget.Others, "Reload");
                        break;
                    case "SPAS-12":
                        PV.RPC("TriggerAnimShotgun", RpcTarget.Others, "Reload");
                        break;
                    case "AK-47":
                        PV.RPC("TriggerAnimAK", RpcTarget.Others, "Reload");
                        break;
                    default:
                        break;
                }
            }
=======
       /* if (!this.gameObject) //1
        {
            GameObject pistol = player.GetComponentInChildren<Gun>().gameObject;
        }*/
        //Debug.Log("10-20: exit btn - ReloadInit(): " + this.gameObject);
        //Debug.Log(PhotonNetwork.LocalPlayer); 
        if(!gunData.isReloading && gunData.magSize != gunData.currentAmmo && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
            animator.SetTrigger("Reload");
            PV.RPC("triggerAnim", RpcTarget.Others, "Reload");
>>>>>>> Blake_Brooks(Current)
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
<<<<<<< HEAD
                            if (hitInfo.collider.tag == "Player" && hitInfo.collider != thisCollider)
                            {
                                Debug.Log("Hit");
                                StartCoroutine(playerHit());
                                hitInfo.transform.GetComponent<PhotonView>().RPC("DamagePlayer", RpcTarget.AllBuffered, gunData.damage);
                            }
                            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                            Debug.Log(hitInfo);
                            damageable?.Damage(gunData.damage);
                        }
=======
                            //Debug.Log("Hit");
                            StartCoroutine(playerHit());
                            
                            hitInfo.transform.GetComponent<PhotonView>().RPC("DamagePlayer", RpcTarget.AllBuffered, gunData.damage, PV.ViewID);
                        }
                        //IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                        //Debug.Log(hitInfo);
                        //damageable?.Damage(gunData.damage);
>>>>>>> Blake_Brooks(Current)
                    }
                    else
                    {
                        bool tempHit = false;
                        for (var i = 0; i < 12; i++)
                        {
                            Vector2 localOffset = playerOrientation.transform.position;
                            float randomX = Random.Range(-.1f, .1f);
                            float randomY = Random.Range(-.1f, .1f);
                            localOffset.y += randomY;
                            localOffset.x += randomX;
                            if (Physics.Raycast(localOffset, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                            {
                                if (hitInfo.collider.tag == "Player" && hitInfo.collider != thisCollider)
                                {
                                    tempHit = true;
                                    Debug.Log("Hit");
                                    hitInfo.transform.GetComponent<PhotonView>().RPC("DamagePlayer", RpcTarget.AllBuffered, gunData.damage);
                                }
                                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                                Debug.Log(hitInfo);
                                damageable?.Damage(gunData.damage);
                            }
                        }
                        if(tempHit) StartCoroutine(playerHit());
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
<<<<<<< HEAD
        if (PV) {
            if (PV.IsMine) {
                timeSinceLastShot += Time.deltaTime;
                //Debug.DrawRay(playerOrientation.transform.position, transform.forward);

                if (transform.parent != null && !settingsOpen && equipped)
                {
                    //ammoCounter = GameObject.Find("AmmoCounter").GetComponent<Text>();
                    if (gunData.reserveAmmo == -1) ammoCounter.text = gunData.currentAmmo.ToString() + "/\u221e";
                    else ammoCounter.text = gunData.currentAmmo.ToString() + "/" + gunData.reserveAmmo.ToString();
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                }
            }
=======
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(playerOrientation.transform.position, transform.forward);
        if (PV.IsMine) { 
        if (transform.parent != null)
        {
            //Debug.Log(transform.parent.parent.parent.GetComponent<PhotonView>().ViewID + "is updating in guncs");
            //ammoCounter = GameObject.Find("AmmoCounter").GetComponent<Text>();
            if (gunData.reserveAmmo == -1) ammoCounter.text = gunData.currentAmmo.ToString() + "/\u221e";
            else ammoCounter.text = gunData.currentAmmo.ToString() + "/" + gunData.reserveAmmo.ToString();
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
>>>>>>> Blake_Brooks(Current)
        }
        }
    }

    private void OnGunShot()
    {
        if (this.gameObject.activeSelf) {
            flash.Play();
            audioSource.clip = gunshot;
            audioSource.Play();
            animator.SetTrigger("Shoot");
            switch(gunData.name) {
                case "M1911":
                    PV.RPC("TriggerAnimPistol", RpcTarget.Others, "Shoot");
                    break;
                case "SPAS-12":
                    PV.RPC("TriggerAnimShotgun", RpcTarget.Others, "Shoot");
                    break;
                case "AK-47":
                    PV.RPC("TriggerAnimAK", RpcTarget.Others, "Shoot");
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator playerHit()
    {
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(.25f);
        hitMarker.SetActive(false);
    }
}
