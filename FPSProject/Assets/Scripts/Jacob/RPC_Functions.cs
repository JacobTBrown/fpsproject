using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_Functions : MonoBehaviour
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

    [PunRPC]
    public void TriggerAnimPistol(string s) {
        pistolAnim.SetTrigger(s);
        if (s == "Reload") {
            pistol.clip = gunPistol.reload;
        } else if (s == "Shoot") {
            pistol.clip = gunPistol.gunshot;
        }
        pistol.Play();
    }

    [PunRPC]
    public void TriggerAnimShotgun(string s) {
        shotgunAnim.SetTrigger(s);
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
}
