using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RPC_Functions : MonoBehaviourPunCallbacks
{
    public WeaponSwap weaponSwap;
    [SerializeField]
    private Animator pistolAnim;
    [SerializeField]
    private Animator shotgunAnim;
    [SerializeField]
    private Animator akAnim;
    public Gun gunPistol;
    public Gun gunShotgun;
    public Gun gunAk;
    public AudioSource pistol;
    public AudioSource shotgun;
    public AudioSource ak;

<<<<<<< HEAD
    [PunRPC]
    public void TriggerAnimPistol(string s) {
        pistolAnim.SetTrigger(s);
        if (s == "Reload") {
            pistol.clip = gunPistol.reload;
        } else if (s == "Shoot") {
            pistol.clip = gunPistol.gunshot;
        }
        pistol.Play();
=======
    public void Start() {
        animator = GetComponentInChildren<Animator>();
        Debug.Log(animator.name + " was animator");
    }
    public void Update()
    {
        if (!animator)
        {
            Debug.Log("animator was null in RPC_Functions.cs");
        }
>>>>>>> Blake_Brooks(Current)
    }
    [PunRPC]
<<<<<<< HEAD
    public void TriggerAnimShotgun(string s) {
        shotgunAnim.SetTrigger(s);
=======
    public void triggerAnim(string s) {
        if (s.Length == 0)
        {
            Debug.Log("bad animation call");
            return;
        }
        //Debug.Log("triggered anim " + s);
        //Debug.Log("animator ws " + animator.name + " reload was " + reload.name);
        animator.SetTrigger(s);
>>>>>>> Blake_Brooks(Current)
        if (s == "Reload") {
            shotgun.clip = gunShotgun.reload;
        } else if (s == "Shoot") {
            shotgun.clip = gunShotgun.gunshot;
        }
        shotgun.Play();
    }

    [PunRPC]
    public void TriggerAnimAK(string s) {
        akAnim.SetTrigger(s);
        if (s == "Reload") {
            ak.clip = gunAk.reload;
        } else if (s == "Shoot") {
            ak.clip = gunAk.gunshot;
        }
        ak.Play();
    }

    [PunRPC]
    public void SetCurrentWeapon(int i) {
        weaponSwap.selected = i;
    }
    [PunRPC]
    public void ClearRPCs(Player o)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " cleared rpcs for " + o.ActorNumber);
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.RemoveRPCs(o);
    }
}
