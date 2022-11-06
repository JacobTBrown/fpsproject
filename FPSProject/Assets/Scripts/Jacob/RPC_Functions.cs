using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RPC_Functions : MonoBehaviourPunCallbacks
{
    public PlayerMovement playerMove;
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
        //print("weapon swapped: " + weaponSwap.selected + " i: " + i);
    }

    [PunRPC]
    public void SetPickupWeapon(int i, bool b, string name) {
        weaponSwap.selected = i;
        if (name == "AK-47") {
            gunAk.owns = b;
        }
        if (name == "SPAS-12") {
            gunShotgun.owns = b;
        }
        //print("weapon swapped: " + weaponSwap.selected + " i: " + i);
    }

    [PunRPC]
    public void AddSpeed(string name, int speed) {
        if (name == "MoveSpeed")
            playerMove.walkSpeed += speed;
        else if (name == "JumpSpeed")
            playerMove.jumpForce += speed;
        else
            playerMove.airSpeed += speed;
    }

    [PunRPC]
    public void SubtractSpeed(string name, int speed) {
        if (name == "MoveSpeed")
            playerMove.walkSpeed -= speed;
        else if (name == "JumpSpeed")
            playerMove.jumpForce -= speed;
        else
            playerMove.airSpeed -= speed;
    }

    [PunRPC]
    public void ClearRPCs(Player o)
    {
        Debug.Log(PhotonNetwork.MasterClient + " cleared rpcs for " + o.ActorNumber);
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.RemoveRPCs(o);
    }
}
