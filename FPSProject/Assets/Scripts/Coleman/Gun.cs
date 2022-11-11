﻿using Photon.Pun;
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
    public bool owns;
    public bool isInstakill;
    Animator animator;
    public AudioClip gunshot;
    public AudioClip reload;
    public AudioSource audioSource;
    public PlayerStatsPage pstats;
    public PlayerShoot shoot;

    //private RPC_Functions rpcFunc;

    private RPC_Functions rpcFunc;

    float timeSinceLastShot;

    private void Start()
    {
        pstats = GameObject.Find("RoomManager").GetComponent<PlayerStatsPage>();
        //Debug.Log(pstats.gameObject.name);
        //Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        if (equipped && transform.parent != null) // && PV.IsMine)
        {
            if (ammoCounter)
            {
                return;
            }
            equipped = true;            
            ammoCounter = GameObject.Find("AmmoCounter").GetComponent<Text>();
            hitMarker = GameObject.Find("HitMarker");
            if (hitMarker) { 
            hitMarker.SetActive(false);
            }
            shoot.shootInput += Shoot;
            shoot.reloadInput += ReloadInit;
            animator = GetComponent<Animator>();
        } else
        {
            equipped = false;
        }
        isInstakill = false;

        gunData.currentAmmo = gunData.magSize;
        if (gunData.maxReserveAmmo != -1) gunData.reserveAmmo = gunData.magSize * 2;
    }

    private IEnumerator Reload()
    {
        gunData.isReloading = true;
        audioSource.clip = reload;
        audioSource.Play();
        yield return new WaitForSeconds(gunData.reloadTime);
        if (gunData.reserveAmmo != -1)
        {
            if(gunData.magSize - gunData.currentAmmo > gunData.reserveAmmo)
            {
                gunData.currentAmmo += gunData.reserveAmmo;
                gunData.reserveAmmo = 0;
            } else
            {
                gunData.reserveAmmo -= gunData.magSize - gunData.currentAmmo;
                gunData.currentAmmo = gunData.magSize;
            }
        } else
        {
            gunData.currentAmmo = gunData.magSize;
        }
        gunData.isReloading = false;
    }

    private void OnDisable() => gunData.isReloading = false;

    public void ReloadInit()
    {
       /* if (!this.gameObject) //1
        {
            GameObject pistol = player.GetComponentInChildren<Gun>().gameObject;
        }*/
        //Debug.Log("10-20: exit btn - ReloadInit(): " + this.gameObject);
        //Debug.Log(PhotonNetwork.LocalPlayer); 
        if(!gunData.isReloading && gunData.magSize != gunData.currentAmmo && gunData.reserveAmmo != 0 && this.gameObject.activeSelf)
        {
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
                    //if (!gunData.isShotgun)
                    //{
                        if (Physics.Raycast(playerOrientation.transform.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                        {
                            PhotonView EPV = hitInfo.transform.GetComponent<PhotonView>();
                            if (hitInfo.collider.tag == "Player" && hitInfo.collider != thisCollider)
                            {

                                if (pstats.CheckTeam(EPV))
                                {
                                    //Debug.Log("Player was on your team: " + EPV.ViewID + " vs " + PV.ViewID);
                                }
                                else
                                {
                                    //Debug.Log("Hit");
                                    StartCoroutine(playerHit());
                                    if (isInstakill)
                                        hitInfo.transform.GetComponent<PhotonView>().RPC("DamagePlayer", RpcTarget.AllBuffered, 120f, PV.ViewID);
                                    else
                                        hitInfo.transform.GetComponent<PhotonView>().RPC("DamagePlayer", RpcTarget.AllBuffered, gunData.damage, PV.ViewID);
                                }
                                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                                Debug.Log(hitInfo);
                                if (isInstakill) {
                                    damageable?.Damage(120f, EPV.ViewID);
                                }    
                                else
                                    damageable?.Damage(gunData.damage, EPV.ViewID);
                            }
                        }
                    //}
                    //else
                    //{
                    //    float maxspread = .1f;
                    //    bool temphit = false;
                    //    for (var i = 0; i < 12; i++)
                    //    {
                    //        vector3 dir = transform.forward + new vector3(random.range(-maxspread, maxspread), random.range(-maxspread, maxspread), random.range(-maxspread, maxspread) + 1);
                    //        if (physics.raycast(dir, transform.forward, out raycasthit hitinfo2, gundata.maxdistance))
                    //        {
                    //            photonview epv = hitinfo2.transform.getcomponent<photonview>();
                    //            if (hitinfo2.collider.tag == "player" && hitinfo2.collider != thiscollider)
                    //            {
                    //                if (pstats.checkteam(epv))
                    //                {
                    //                    debug.log("player was on your team: " + epv.viewid + " vs " + pv.viewid);
                    //                }
                    //                else
                    //                {
                    //                    temphit = true;
                    //                    debug.log("shotgun: " + epv.viewid);
                    //                    hitinfo2.transform.getcomponent<photonview>().rpc("damageplayer", rpctarget.allbuffered, gundata.damage, epv.viewid);
                    //                }
                    //                idamageable damageable = hitinfo2.transform.getcomponent<idamageable>();
                    //                debug.log(hitinfo2);
                    //                damageable?.damage(gundata.damage, epv.viewid);
                    //            }
                    //        }
                    //    }
                    //    if (temphit) startcoroutine(playerhit());
                    //}
                    gunData.currentAmmo--;
                    timeSinceLastShot = 0;
                    OnGunShot();
                }
            }
        }
    }

    private void Update()
    {
        if (PV) {
            if (PV.IsMine) {
                timeSinceLastShot += Time.deltaTime;
                //Debug.DrawRay(playerOrientation.transform.position, transform.forward);
                if (equipped == true) owns = true;
                if (transform.parent != null && !settingsOpen && equipped)
                {
                    //ammoCounter = GameObject.Find("AmmoCounter").GetComponent<Text>();
                    if (gunData.reserveAmmo == -1) ammoCounter.text = gunData.currentAmmo.ToString() + "/\u221e";
                    else ammoCounter.text = gunData.currentAmmo.ToString() + "/" + gunData.reserveAmmo.ToString();
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                }
            }
            //if (transform.position != Vector3.zero) transform.position.normalized;
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
